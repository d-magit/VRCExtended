using System;
using System.Collections.Generic;
using System.IO;
using VRCExtended.Utils;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
    private static void GetLocals()
    {
        if (!Utilities.EnsureFolderExists(Utilities.ModulesFolder))
            return;

        IEnumerable<string> filePaths;
        try
        { filePaths = Directory.GetFiles(Utilities.ModulesFolder, "*.dll"); }
        catch
        { return; }

        foreach (var path in filePaths)
        {
            // Checks first if assembly already is on GAC.
            var assembly = AsmManager.GetIfExists(AppDomain.CurrentDomain, path);
            
            Entry entry;
            (string Name, Version Version) assemblyInfo;
            if (assembly != null)
            {
                var asmName = assembly.GetName();
                assemblyInfo = (asmName.Name, asmName.Version);
                
                entry = new Entry
                {
                    Type = EntryType.Gac,
                    Assembly = assembly,
                    Version = assemblyInfo.Version
                };
            }
            else
            {
                if (!AsmManager.Instance.TryGetInfo(path, out assemblyInfo))
                    continue;
                
                entry = new Entry
                {
                    Type = EntryType.Local,
                    Path = path,
                    Version = assemblyInfo.Version
                };
            }
            
            GetModuleEntries(assemblyInfo.Name).Add(entry);
        }
        
        // Attempts to unload assemblies to avoid memory leaks
        AsmManager.Instance = null;
    }
}