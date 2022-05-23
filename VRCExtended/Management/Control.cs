using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerRuntimeLib;
using VRCExtended.Base;

namespace VRCExtended.Management;

internal static partial class Manager
{
	internal static VRCExtended Instance;
	
	private static readonly Dictionary<Type, VrcMod> Modules = new();
	public static void CacheModule(Type moduleType, VrcMod module) => Modules.Add(moduleType, module);
	public static void InitializeModule<T>() where T : VrcMod, new() => CacheModule(typeof(T), new T());
	public static T GetModule<T>() where T : VrcMod => (T)Modules[typeof(T)];

	internal static void UnloadModules() =>
		Modules.Values.OfType<IOnModUnload>().ToList().ForEach(m => m.OnModUnload());

	internal static void Init(VRCExtended instance)
	{
		Instance = instance;
		Instance.ApplicationLateStart += CustomEvents.Init;
		ClassInjector.RegisterTypeInIl2Cpp<Utils.EnableDisableListener>();
		LoadModules();
	}
}