using System;
using VRCExtended.Management;

namespace VRCExtended.Base;

public abstract partial class VrcMod : IMelonEvents
{
	public virtual void OnPreSupportModule() { }
	public virtual void OnApplicationStart() { }
	public virtual void OnApplicationLateStart() { }
	public virtual void OnApplicationQuit() { }
	public virtual void OnUpdate() { }
	public virtual void OnLateUpdate() { }
	public virtual void OnFixedUpdate() { }
	public virtual void OnGUI() { }
	public virtual void OnPreferencesLoaded(string filePath) { }
	public virtual void OnPreferencesSaved(string filePath) { }
	public virtual void OnSceneWasLoaded(int buildIndex, string sceneName) { }
	public virtual void OnSceneWasInitialized(int buildIndex, string sceneName) { }
	public virtual void OnSceneWasUnloaded(int buildIndex, string sceneName) { }
	
	private void RegisterMelonEvents()
	{
		if (IsOverriding(nameof(OnPreSupportModule))) MelonModDetour.ModInstance.PreSupportModule += () => // TODO: Check if events are canceled on error without the try catch
		{
			try
			{ OnPreSupportModule(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreSupportModule)); }
		};
		if (IsOverriding(nameof(OnApplicationStart))) MelonModDetour.ModInstance.ApplicationStart += () =>
		{
			try
			{ OnApplicationStart(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationStart)); }
		};
		if (IsOverriding(nameof(OnApplicationLateStart))) MelonModDetour.ModInstance.ApplicationLateStart += () =>
		{
			try
			{ OnApplicationLateStart(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationLateStart)); }
		};
		if (IsOverriding(nameof(OnApplicationQuit))) MelonModDetour.ModInstance.ApplicationQuit += () =>
		{
			try
			{ OnApplicationQuit(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationQuit)); }
		};
		if (IsOverriding(nameof(OnUpdate))) MelonModDetour.ModInstance.Update += () =>
		{
			try
			{ OnUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnUpdate)); }
		};
		if (IsOverriding(nameof(OnLateUpdate))) MelonModDetour.ModInstance.LateUpdate += () =>
		{
			try
			{ OnLateUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnLateUpdate)); }
		};
		if (IsOverriding(nameof(OnFixedUpdate))) MelonModDetour.ModInstance.FixedUpdate += () =>
		{
			try
			{ OnFixedUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnFixedUpdate)); }
		};
		if (IsOverriding(nameof(OnGUI))) MelonModDetour.ModInstance.GUI += () =>
		{
			try
			{ OnGUI(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnGUI)); }
		};
		if (IsOverriding(nameof(OnPreferencesLoaded))) MelonModDetour.ModInstance.PreferencesLoaded += filePath =>
		{
			try
			{ OnPreferencesLoaded(filePath); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreferencesLoaded)); }
		};
		if (IsOverriding(nameof(OnPreferencesSaved))) MelonModDetour.ModInstance.PreferencesSaved += filePath =>
		{
			try
			{ OnPreferencesSaved(filePath); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreferencesSaved)); }
		};
		if (IsOverriding(nameof(OnSceneWasLoaded))) MelonModDetour.ModInstance.SceneWasLoaded += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasLoaded(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasLoaded)); }
		};
		if (IsOverriding(nameof(OnSceneWasInitialized))) MelonModDetour.ModInstance.SceneWasInitialized += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasInitialized(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasInitialized)); }
		};
		if (IsOverriding(nameof(OnSceneWasUnloaded))) MelonModDetour.ModInstance.SceneWasUnloaded += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasUnloaded(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasUnloaded)); }
		};
	}
}