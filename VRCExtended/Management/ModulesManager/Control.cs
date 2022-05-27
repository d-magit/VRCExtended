using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using VRCExtended.Mod;

namespace VRCExtended.Management;

internal static partial class ModulesManager
{
	private static readonly Dictionary<Type, MelonBase> Modules = new();
	public static void CacheModule(Type moduleType, MelonBase module) => Modules.Add(moduleType, module);
	public static void InitializeModule<T>() where T : MelonBase, new() => CacheModule(typeof(T), new T());
	public static T GetModule<T>() where T : MelonBase => (T)Modules[typeof(T)];
	
	private static readonly Dictionary<string, List<Entry>> ToSelectEntries = new();
	private static List<Entry> GetModuleEntries(string moduleName)
	{
		if (ToSelectEntries.TryGetValue(moduleName, out var moduleEntry))
			return moduleEntry;
        
		moduleEntry = new List<Entry>();
		ToSelectEntries.Add(moduleName, moduleEntry);
		return moduleEntry;
	}

	internal static void ReadyModules()
	{
		// TODO: Use MelonInfo attr for versions.
		GetLocals();
		GetRemotes();
		SelectAndReady();
		Main.Logger.Msg(ConsoleColor.Green, "Modules are ready to load!");
	}

	// TODO: Add in-game UI to load and unload modules.
	internal static void UnloadModules() =>
		Detour.Mod.OnUnload();
    
	internal static void LoadModules()
	{
		// TODO: Make this run on a different thread asynchronously? Might have to check interactions with UiManagerInit. Possibly add custom attribute for this.
		foreach (var entry in ToLoadEntries.OrderBy(e => e.Priority))
			LoadModule(entry.Assembly);
		
		foreach (var melon in Modules.Values)
			EventSubscriber.Subscribe(melon);
		
		Main.Logger.Msg(ConsoleColor.Green, "Modules loaded successfully!");
	}
}