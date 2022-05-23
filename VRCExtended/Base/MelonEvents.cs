using System;

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
		if (IsOverriding(nameof(OnPreSupportModule))) Management.Manager.Instance.PreSupportModule += () =>
		{
			try
			{ OnPreSupportModule(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreSupportModule)); }
		};
		if (IsOverriding(nameof(OnApplicationStart))) Management.Manager.Instance.ApplicationStart += () =>
		{
			try
			{ OnApplicationStart(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationStart)); }
		};
		if (IsOverriding(nameof(OnApplicationLateStart))) Management.Manager.Instance.ApplicationLateStart += () =>
		{
			try
			{ OnApplicationLateStart(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationLateStart)); }
		};
		if (IsOverriding(nameof(OnApplicationQuit))) Management.Manager.Instance.ApplicationQuit += () =>
		{
			try
			{ OnApplicationQuit(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnApplicationQuit)); }
		};
		if (IsOverriding(nameof(OnUpdate))) Management.Manager.Instance.Update += () =>
		{
			try
			{ OnUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnUpdate)); }
		};
		if (IsOverriding(nameof(OnLateUpdate))) Management.Manager.Instance.LateUpdate += () =>
		{
			try
			{ OnLateUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnLateUpdate)); }
		};
		if (IsOverriding(nameof(OnFixedUpdate))) Management.Manager.Instance.FixedUpdate += () =>
		{
			try
			{ OnFixedUpdate(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnFixedUpdate)); }
		};
		if (IsOverriding(nameof(OnGUI))) Management.Manager.Instance.GUI += () =>
		{
			try
			{ OnGUI(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnGUI)); }
		};
		if (IsOverriding(nameof(OnPreferencesLoaded))) Management.Manager.Instance.PreferencesLoaded += filePath =>
		{
			try
			{ OnPreferencesLoaded(filePath); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreferencesLoaded)); }
		};
		if (IsOverriding(nameof(OnPreferencesSaved))) Management.Manager.Instance.PreferencesSaved += filePath =>
		{
			try
			{ OnPreferencesSaved(filePath); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPreferencesSaved)); }
		};
		if (IsOverriding(nameof(OnSceneWasLoaded))) Management.Manager.Instance.SceneWasLoaded += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasLoaded(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasLoaded)); }
		};
		if (IsOverriding(nameof(OnSceneWasInitialized))) Management.Manager.Instance.SceneWasInitialized += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasInitialized(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasInitialized)); }
		};
		if (IsOverriding(nameof(OnSceneWasUnloaded))) Management.Manager.Instance.SceneWasUnloaded += (buildIndex, sceneName) =>
		{
			try
			{ OnSceneWasUnloaded(buildIndex, sceneName); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnSceneWasUnloaded)); }
		};
	}
}