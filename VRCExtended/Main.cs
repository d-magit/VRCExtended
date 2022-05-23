#region Imports, Info & Namespace
using System;
using System.Reflection;
using MelonLoader;
using VRC;
using VRC.Core;

using BuildInfo = VRCExtended.BuildInfo;
[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyCopyright($"Created by {BuildInfo.Author}")]
[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]

namespace VRCExtended;

public static class BuildInfo
{
    public const string Name = "VRCExtended";
    public const string Author = "Elaina";
    public const string Version = "1.0.0";
    public const ConsoleColor MelonColor = ConsoleColor.DarkMagenta;
}
#endregion

public partial class VRCExtended : MelonMod, IMelonEvents, ICustomEvents
{
    public static readonly MelonLogger.Instance Logger = new(BuildInfo.Name, BuildInfo.MelonColor); //TODO: Remove this
    public VRCExtended()
    {
        MelonPreferences.CreateCategory(BuildInfo.Name, $"{BuildInfo.Name} - Base");
        Management.Manager.Init(this);
        Logger.Msg(ConsoleColor.Green, "Successfully loaded!");
    }
    
    #region Melon Events
    public override void OnPreSupportModule() => PreSupportModule?.Invoke();
    public override void OnApplicationStart() => ApplicationStart?.Invoke();
    public override void OnApplicationLateStart() => ApplicationLateStart?.Invoke();
    // TODO: Save settings on application quit.
    public override void OnApplicationQuit() => ApplicationQuit?.Invoke();
    public override void OnUpdate() => Update?.Invoke();
    public override void OnLateUpdate() => LateUpdate?.Invoke();
    public override void OnFixedUpdate() => FixedUpdate?.Invoke();
    public override void OnGUI() => GUI?.Invoke();
    public override void OnPreferencesLoaded(string filePath) => PreferencesLoaded?.Invoke(filePath);
    public override void OnPreferencesSaved(string filePath) => PreferencesSaved?.Invoke(filePath);
    public override void OnSceneWasLoaded(int buildIndex, string sceneName) => SceneWasLoaded?.Invoke(buildIndex, sceneName);
    public override void OnSceneWasInitialized(int buildIndex, string sceneName) => SceneWasInitialized?.Invoke(buildIndex, sceneName);
    public override void OnSceneWasUnloaded(int buildIndex, string sceneName) => SceneWasUnloaded?.Invoke(buildIndex, sceneName);
    #endregion

    #region Custom Events
    // TODO: Add Unity and VRChat UiManagerInits.
    public void OnUiManagerInit() => UiManagerInit?.Invoke();
    public void OnPlayerJoined(Player player) => PlayerJoined?.Invoke(player);
    public void OnPlayerLeft(Player player) => PlayerLeft?.Invoke(player);
    public void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance) => InstanceChanged?.Invoke(world, instance);
    #endregion
}