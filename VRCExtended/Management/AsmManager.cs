using System;
using System.Linq;
using System.Reflection;

namespace VRCExtended.Management;

internal class AsmManager : MarshalByRefObject
{
    private static AsmManager _instance;
    internal static AsmManager Instance
    {
        get => _instance ??= (AsmManager)
            (CachedLocals ??= AppDomain.CreateDomain(
                BuildInfo.Name, 
                AppDomain.CurrentDomain.Evidence, 
                new AppDomainSetup {
                    ApplicationBase = Environment.CurrentDirectory,
                    PrivateBinPath = string.Join(";", $"{BuildInfo.Name}/Modules", "Mods", "Plugins"),
                    LoaderOptimization = LoaderOptimization.MultiDomainHost })
            ).CreateInstanceAndUnwrap(
                typeof(AsmManager).Assembly.FullName,
                typeof(AsmManager).FullName!
            );
        
        set
        {
            if (value != null)
            {
                _instance = value;
                return;
            }

            CachedLocals = null;
            if (_cachedLocals != null)
            {
                Main.Logger.Warning("Failed to remove AsmManagement.");
                return;
            }
            
            _instance = null;
        }
    }
    internal static bool Exists() => _instance != null;
    
    private static AppDomain _cachedLocals;
    internal static AppDomain CachedLocals
    {
        get => _cachedLocals;
        set
        {
            try
            {
                if (value == null && _cachedLocals != null)
                    AppDomain.Unload(_cachedLocals);

                _cachedLocals = value;
            }
            catch (Exception e)
            {
                Main.Logger.Warning($"Failed to unload CachedLocals domain. Error: {e}");
            }
        }
    }
    
    public static Assembly GetIfExists(AppDomain dom, string path) => dom.GetAssemblies().FirstOrDefault(i =>
    {
        try
        { return i.Location.Equals(path, StringComparison.OrdinalIgnoreCase); }
        catch
        { return false; }
    });
    
    public bool TryGetInfo(string path, out (string Name, Version Version) info)
    {
        info = default;
        
        try
        {
            var assembly = Assembly.LoadFrom(path);
            var asmName = assembly.GetName();
            info = (asmName.Name, asmName.Version);
            return true;
        }
        catch
        {
            Main.Logger.Warning($"Failed to get info for assembly in {path}.");
            return false;
        }
    }
}