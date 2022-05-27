using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using VRCExtended.Utils;
using Newtonsoft.Json;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
    private static void GetRemotes()
    {
        var providers = Settings.Instance.Modules.GroupBy(m => m.Provider).ToList();
        var selfHosted = providers.FirstOrDefault(p => string.IsNullOrEmpty(p.Key));
        if (selfHosted != null)
        {
            providers.Remove(selfHosted);
            // TODO: Enqueue self hosted modules. Make sure it's thread safe and low priority.
        }
        
        providers.ForEach(GetFromRemoteProvider);
    }

    private static void GetFromRemoteProvider(IGrouping<string, ConfigEntry> provider)
    {
        if (!Settings.Instance.Providers.TryGetValue(provider.Key, out var providerAddress))
        {
            Main.Logger.Warning($"Failed to get all modules from provider {provider.Key}: Provider not found.");
            return;
        }

        Uri providerUri;
        try
        { providerUri = new Uri(providerAddress); }
        catch
        {
            Main.Logger.Warning($"Failed to get all modules from provider {provider.Key}: Provided URI is invalid.");
            return;
        }
        
        var moduleNames = provider.Select(m => m.AssemblyName).ToArray();
        var versions = GetRemoteVersions(providerUri, moduleNames);
        if (versions == null)
            Main.Logger.Warning($"Failed to get all modules from provider {provider.Key}: Failed to get versions.");
        else
        { // TODO: Debug here
            foreach (var module in moduleNames)
            {
                if (!versions.TryGetValue(module, out var version))
                    continue;

                var moduleEntry = new Entry
                {
                    Type = EntryType.Remote,
                    Path = new Uri(providerUri, $"{module}.dll").ToString(),
                    Version = version
                };
                GetModuleEntries(module).Add(moduleEntry);
            }
        }
    }
    
    private static Dictionary<string, Version> GetRemoteVersions(Uri uri, string[] data) {
        try
        {
            return JsonConvert.DeserializeObject<Dictionary<string, Version>>(
                new WebClient {Headers = {[HttpRequestHeader.ContentType] = "application/json"}}
                    .UploadString(uri, JsonConvert.SerializeObject(data)))!;
        }
        catch
        { return null; }
    }
    
    private static Assembly GetRemoteAssembly(Entry entry)
    {
        byte[] bytes = null;
        try
        { bytes = new WebClient().DownloadData(entry.Path); }
        catch
        { /* ignored */ }
        
        if (bytes == null) 
            return null;

        try
        {
            var loadedAsm = Assembly.Load(bytes);
            
            if (!Utilities.EnsureFolderExists(Utilities.ModulesFolder)) 
                return loadedAsm;
            
            try
            { File.WriteAllBytes(Path.Combine(Utilities.ModulesFolder, $"{loadedAsm.GetName().Name}.dll"), bytes); } 
            catch 
            { /* ignored */ }
            
            return loadedAsm;
        }
        catch { return null; }
    }
}