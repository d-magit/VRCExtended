using System;
using MelonLoader;
using VRCExtended.Management;

namespace VRCExtended.Mod;

// ReSharper disable once InconsistentNaming
public partial class DummyMod : MelonMod, IModEvents, IBaseEvents
{
    public DummyMod() => ModulesManager.LoadModules(); // TODO: Change this when adding Plugins
    
    public override void OnPreSupportModule() => PreSupportModule?.Invoke();
    public override void OnApplicationStart() => ApplicationStart?.Invoke();
    public override void OnApplicationLateStart() => ApplicationLateStart?.Invoke();
    // TODO: Save settings on application quit.
    public override void OnApplicationQuit() => ApplicationQuit?.Invoke();
    public override void OnUpdate() => Update?.Invoke();
    public override void OnLateUpdate() => LateUpdate?.Invoke();
    public override void OnFixedUpdate() => FixedUpdate?.Invoke();
    public override void OnGUI() => GUI?.Invoke();
    // TODO: Add other preferences and missing overrides.
    public override void OnPreferencesLoaded(string filePath) => PreferencesLoaded?.Invoke(filePath);
    public override void OnPreferencesSaved(string filePath) => PreferencesSaved?.Invoke(filePath);
    public override void OnSceneWasLoaded(int buildIndex, string sceneName) => SceneWasLoaded?.Invoke(buildIndex, sceneName);
    public override void OnSceneWasInitialized(int buildIndex, string sceneName) => SceneWasInitialized?.Invoke(buildIndex, sceneName);
    public override void OnSceneWasUnloaded(int buildIndex, string sceneName) => SceneWasUnloaded?.Invoke(buildIndex, sceneName);
    
    internal event Action PreSupportModule;
    internal event Action ApplicationStart;
    internal event Action ApplicationLateStart;
    internal event Action ApplicationQuit;
    internal event Action Update;
    internal event Action LateUpdate;
    internal event Action FixedUpdate;
    internal event Action GUI;
    internal event Action<string> PreferencesLoaded;
    internal event Action<string> PreferencesSaved;
    internal event Action<int, string> SceneWasLoaded;
    internal event Action<int, string> SceneWasInitialized;
    internal event Action<int, string> SceneWasUnloaded;
}

internal interface IModEvents
{
    void OnFixedUpdate();
    void OnSceneWasLoaded(int buildIndex, string sceneName);
    void OnSceneWasInitialized(int buildIndex, string sceneName);
    void OnSceneWasUnloaded(int buildIndex, string sceneName);
}
internal interface IBaseEvents
{
    void OnPreSupportModule();
    void OnApplicationStart();
    void OnApplicationLateStart();
    void OnApplicationQuit();
    void OnUpdate();
    void OnLateUpdate();
    void OnGUI();
    void OnPreferencesLoaded(string filePath);
    void OnPreferencesSaved(string filePath);
}