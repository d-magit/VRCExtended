using System;
using System.Collections.Generic;
using System.Linq;
using VRCExtended.Base;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
	private static readonly Dictionary<Type, VrcMod> Modules = new();
	public static void CacheModule(Type moduleType, VrcMod module) => Modules.Add(moduleType, module);
	public static void InitializeModule<T>() where T : VrcMod, new() => CacheModule(typeof(T), new T());
	public static T GetModule<T>() where T : VrcMod => (T)Modules[typeof(T)];
	
	private static readonly Dictionary<string, List<ModuleEntry>> ToSelectEntries = new();
	private static List<ModuleEntry> GetModuleEntries(string moduleName)
	{
		if (ToSelectEntries.TryGetValue(moduleName, out var moduleEntry))
			return moduleEntry;
        
		moduleEntry = new List<ModuleEntry>();
		ToSelectEntries.Add(moduleName, moduleEntry);
		return moduleEntry;
	}

	internal static void ReadyModules()
	{
		GetLocals();
		GetRemotes();
		SelectAndReady();
	}

	// TODO: Add in-game UI to load and unload modules.
	// ReSharper disable once SuspiciousTypeConversion.Global
	internal static void UnloadModules() =>
		Modules.Values.OfType<IOnModUnload>().ToList().ForEach(m => m.OnModUnload());
    
	internal static void LoadModules()
	{
		// TODO: Make this run on a different thread asynchronously? Might have to check interactions with UiManagerInit. Possibly add custom attribute for this.
		foreach (var entry in ToLoadEntries.OrderBy(e => e.Priority))
			LoadModule(entry.Assembly);
	}
}