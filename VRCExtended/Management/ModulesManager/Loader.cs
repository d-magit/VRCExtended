using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
    private static readonly List<Entry> ToLoadEntries = new();
    
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
                Main.Logger.Warning($"Failed to load {m.Key} from all resources. This module won't load.");
                continue;
            }
            
            ToLoadEntries.Add(entry);
        }
    }
    
    private static bool TryGetBestPossibleEntry(IEnumerable<IOrderedEnumerable<Entry>> versionsEnum, out Entry result)
    {
        foreach (var versions in versionsEnum)
        {
            foreach (var entry in versions)
            {
                try
                {
                    entry.Assembly = entry.Type switch
                    {
                        EntryType.Gac => entry.Assembly,
                        // If cachedLocals failed to unload, will try to take advantage of it.
                        EntryType.Local when AsmManager.Exists() => AsmManager.GetIfExists(AsmManager.CachedLocals, entry.Path) ?? Assembly.LoadFrom(entry.Path),
                        EntryType.Local when !AsmManager.Exists() => Assembly.LoadFrom(entry.Path),
                        EntryType.Remote => GetRemoteAssembly(entry),
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
        Type melonType = null;
        if (!Utils.Utilities.TryGetAssemblyAttribute<MelonInfoAttribute>(moduleAsm, out var infoAttr) && 
            !TryGetInheritedType(moduleAsm, typeof(MelonBase), out melonType))
        {
            Main.Logger.Warning($"Failed to load assembly {moduleAsm.GetName().FullName}: No MelonBase inherited class found.");
            return;
        }
        melonType ??= infoAttr.SystemType;

        MelonBase instance;
        try
        {
            instance = (MelonBase)Activator.CreateInstance(melonType);
            if (instance == null)
                throw new NullReferenceException();
        }
        catch
        {
            Main.Logger.Warning($"Failed to load MelonBase {melonType.FullName}: Failed to create module instance.");
            return;
        }
        
        CacheModule(melonType, instance);
    }
    
    private static bool TryGetInheritedType(Assembly asm, Type baseType, out Type type)
    {
        type = null;
        
        if (asm == null)
            return false;
    
        IEnumerable<Type> types;
        try { types = asm.GetTypes(); }
        catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null); }
        catch { return false; }
    
        type = types.FirstOrDefault(t => t.IsSubclassOf(baseType));
        return type != null;
    }
}