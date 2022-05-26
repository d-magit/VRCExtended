using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRCExtended.Base;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
    private static readonly List<ModuleEntry> ToLoadEntries = new();
    
    private static void SelectAndReady()
    {
        foreach (var m in ToSelectEntries)
        {
            var groupedByVersions = m.Value
                .GroupBy(a => a.Version)
                .OrderByDescending(a => a.Key)
                .Select(g => g.OrderBy(e => (int)e.Type));

            if (!TryGetBestPossibleEntry(groupedByVersions, out var entry))
            {
                VRCExtendedPlugin.Logger.Warning($"Failed to load {m.Key} from all resources. This module won't load.");
                continue;
            }
            
            ToLoadEntries.Add(entry);
        }
    }
    
    private static bool TryGetBestPossibleEntry(IEnumerable<IOrderedEnumerable<ModuleEntry>> versionsEnum, out ModuleEntry result)
    {
        foreach (var versions in versionsEnum)
        {
            foreach (var entry in versions)
            {
                try
                {
                    entry.Assembly = entry.Type switch
                    {
                        ModuleType.Gac => entry.Assembly,
                        // If cachedLocals failed to unload, will try to take advantage of it.
                        ModuleType.Local when AsmManager.Exists() => AsmManager.GetIfExists(AsmManager.CachedLocals, entry.Path) ?? Assembly.LoadFrom(entry.Path),
                        ModuleType.Local when !AsmManager.Exists() => Assembly.LoadFrom(entry.Path),
                        ModuleType.Remote => GetRemoteAssembly(entry),
                        _ => null
                    };
                } catch { /* ignored */ }
                
                if (entry.Assembly == null) continue;
                result = entry;
                return true;
            }
        }

        result = null;
        return false;
    }
    
    private static void LoadModule(Assembly moduleAsm)
    {
        if (!TryGetInheritedType(moduleAsm, typeof(VrcMod), out var moduleType))
        {
            VRCExtendedPlugin.Logger.Warning($"Failed to load assembly {moduleAsm.GetName().FullName}: No ModuleBase inherited class found.");
            return;
        }

        VrcMod instance;
        try
        {
            instance = (VrcMod)Activator.CreateInstance(moduleType);
            if (instance == null)
                throw new NullReferenceException();
        }
        catch
        {
            VRCExtendedPlugin.Logger.Warning($"Failed to load ModuleBase {moduleType.FullName}: Failed to create module instance.");
            return;
        }
        
        CacheModule(moduleType, instance);
    }
    
    private static bool TryGetInheritedType(Assembly module, Type baseType, out Type type)
    {
        type = null;
        
        if (module == null)
            return false;
    
        IEnumerable<Type> types;
        try { types = module.GetTypes(); }
        catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null); }
        catch { return false; }
    
        type = types.FirstOrDefault(t => t.IsSubclassOf(baseType));
        return type != null;
    }
}