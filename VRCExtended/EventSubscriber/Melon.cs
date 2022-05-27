using System;
using MelonLoader;
using VRCExtended.Mod;

namespace VRCExtended;

public static partial class EventSubscriber
{
	private static void SubscribeMelonEvents(MelonBase melon)
	{
		_currentMelonType = melon.GetType();
		SubscribeBaseEvents(melon);
		switch (melon)
		{
			case MelonMod mod:
				SubscribeModEvents(mod);
				break;
			case MelonPlugin plugin:
				SubscribePluginEvents(plugin);
				break;
			default:
				throw new ArgumentOutOfRangeException($"{melon.GetType().Name} is not a valid MelonBase type.");
		}
	}

	private static void SubscribePluginEvents(MelonPlugin plugin)
	{
		throw new NotImplementedException();
	}

	private static void SubscribeModEvents(MelonMod mod)
	{
		var melonName = mod.Info.Name;
		if (IsOverriding(nameof(mod.OnFixedUpdate)))
			Detour.Mod.FixedUpdate += () =>
			{
				try
				{ mod.OnFixedUpdate(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(mod.OnFixedUpdate)); }
			};
		if (IsOverriding(nameof(mod.OnSceneWasLoaded)))
			Detour.Mod.SceneWasLoaded += (buildIndex, sceneName) =>
			{
				try
				{ mod.OnSceneWasLoaded(buildIndex, sceneName); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(mod.OnSceneWasLoaded)); }
			};
		if (IsOverriding(nameof(mod.OnSceneWasInitialized)))
			Detour.Mod.SceneWasInitialized += (buildIndex, sceneName) =>
			{
				try
				{ mod.OnSceneWasInitialized(buildIndex, sceneName); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(mod.OnSceneWasInitialized)); }
			};
		if (IsOverriding(nameof(mod.OnSceneWasUnloaded)))
			Detour.Mod.SceneWasUnloaded += (buildIndex, sceneName) =>
			{
				try
				{ mod.OnSceneWasUnloaded(buildIndex, sceneName); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(mod.OnSceneWasUnloaded)); }
			};
	}

	private static void SubscribeBaseEvents(MelonBase @base)
	{
		var melonName = @base.Info.Name;
		if (IsOverriding(nameof(@base.OnPreSupportModule)))
			Detour.Mod.PreSupportModule += () =>
			{
				try
				{ @base.OnPreSupportModule(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnPreSupportModule)); }
			};
		if (IsOverriding(nameof(@base.OnApplicationStart)))
			Detour.Mod.ApplicationStart += () =>
			{
				try
				{ @base.OnApplicationStart(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnApplicationStart)); }
			};
		if (IsOverriding(nameof(@base.OnApplicationLateStart)))
			Detour.Mod.ApplicationLateStart += () =>
			{
				try
				{ @base.OnApplicationLateStart(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnApplicationLateStart)); }
			};
		if (IsOverriding(nameof(@base.OnApplicationQuit)))
			Detour.Mod.ApplicationQuit += () =>
			{
				try
				{ @base.OnApplicationQuit(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnApplicationQuit)); }
			};
		if (IsOverriding(nameof(@base.OnUpdate)))
			Detour.Mod.Update += () =>
			{
				try
				{ @base.OnUpdate(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnUpdate)); }
			};
		if (IsOverriding(nameof(@base.OnLateUpdate)))
			Detour.Mod.LateUpdate += () =>
			{
				try
				{ @base.OnLateUpdate(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnLateUpdate)); }
			};
		if (IsOverriding(nameof(@base.OnGUI)))
			Detour.Mod.GUI += () =>
			{
				try
				{ @base.OnGUI(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnGUI)); }
			};
		if (IsOverriding(nameof(@base.OnPreferencesLoaded)))
			Detour.Mod.PreferencesLoaded += filePath =>
			{
				try
				{ @base.OnPreferencesLoaded(filePath); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnPreferencesLoaded)); }
			};
		if (IsOverriding(nameof(@base.OnPreferencesSaved)))
			Detour.Mod.PreferencesSaved += filePath =>
			{
				try
				{ @base.OnPreferencesSaved(filePath); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(@base.OnPreferencesSaved)); }
			};
	}
}