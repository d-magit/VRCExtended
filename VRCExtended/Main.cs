#region Imports, Info & Namespace
using System;
using System.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib;
using VRCExtended.Management;
using VRCExtended.Mod;
using VRCExtended.Utils;

using BuildInfo = VRCExtended.BuildInfo;
[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyCopyright($"Created by {BuildInfo.Author}")]
[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]
[assembly: MelonInfo(typeof(VRCExtended.Main), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author)]
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
// TODO: Add plugins support
public class Main : MelonPlugin
{
    internal static MelonLogger.Instance Logger = new(BuildInfo.Name, BuildInfo.MelonColor);
    internal new static HarmonyLib.Harmony Harmony;
    internal Main() => ModulesManager.ReadyModules();

    public override void OnPreInitialization()
    {
        Harmony = HarmonyInstance;
        Logger = LoggerInstance;
        Detour.InjectMelon();
    }

    public override void OnApplicationLateStart()
    {
        CustomEvents.Init();
        ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
    }
    
    public override void OnApplicationQuit() =>
        Settings.SaveConfig();
}