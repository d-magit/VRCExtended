using System;
using VRC;
using VRC.Core;

namespace VRCExtended;

public partial class VRCExtended
{
    #region Melon Events
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
    #endregion

    #region Custom Events
    internal event Action UiManagerInit;
    internal event Action<Player> PlayerJoined;
    internal event Action<Player> PlayerLeft;
    internal event Action<ApiWorld, ApiWorldInstance> InstanceChanged;
    #endregion
}

internal interface IMelonEvents
{
    void OnPreSupportModule();
    void OnApplicationStart();
    void OnApplicationLateStart();
    void OnApplicationQuit();
    void OnUpdate();
    void OnLateUpdate();
    void OnFixedUpdate();
    void OnGUI();
    void OnPreferencesLoaded(string filePath);
    void OnPreferencesSaved(string filePath);
    void OnSceneWasLoaded(int buildIndex, string sceneName);
    void OnSceneWasInitialized(int buildIndex, string sceneName);
    void OnSceneWasUnloaded(int buildIndex, string sceneName);
}

internal interface ICustomEvents
{
    void OnUiManagerInit();
    void OnPlayerJoined(Player player);
    void OnPlayerLeft(Player player);
    void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance);
}