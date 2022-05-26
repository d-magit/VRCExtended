#region Imports, Info & Namespace
using System;
using System.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib;
using VRC;
using VRC.Core;
using VRCExtended.Management;
using VRCExtended.Utils;

using BuildInfo = VRCExtended.BuildInfo;
[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyCopyright($"Created by {BuildInfo.Author}")]
[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]
[assembly: MelonInfo(typeof(VRCExtended.VRCExtendedPlugin), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(BuildInfo.MelonColor)]
[assembly: MelonPriority(int.MinValue)]

namespace VRCExtended;

public static class BuildInfo
{
    public const string Name = "VRCExtended";
    public const string Author = "Elaina";
    public const string Version = "1.0.0";
    public const ConsoleColor MelonColor = ConsoleColor.DarkMagenta;
}
#endregion

// ReSharper disable once InconsistentNaming
public class VRCExtendedPlugin : MelonPlugin
{
    private static VRCExtendedPlugin _instance;
    internal static MelonLogger.Instance Logger = new(BuildInfo.Name, BuildInfo.MelonColor);
    internal new static HarmonyLib.Harmony Harmony => _instance.HarmonyInstance;
    internal VRCExtendedPlugin()
    {
        _instance = this;
        ModulesManager.ReadyModules();
    }

    public override void OnPreInitialization()
    {
        MelonModDetour.InjectMelon();
        (Logger = LoggerInstance).Msg(ConsoleColor.Green, "Successfully loaded!");
    }

    public override void OnApplicationLateStart()
    {
        CustomEvents.Init();
        ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
    }
    
    public override void OnApplicationQuit() =>
        Settings.SaveConfig();
}

// ReSharper disable once InconsistentNaming
public partial class VRCExtendedMod : MelonMod, IMelonEvents, ICustomEvents
{
    public VRCExtendedMod() => ModulesManager.LoadModules();
    
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