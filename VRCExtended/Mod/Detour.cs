using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;

namespace VRCExtended.Mod;

internal static class Detour
{
    internal static DummyMod Mod;
    
    internal static void InjectMelon()
    {
        var modsField = typeof(MelonHandler).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(p => p.FieldType == typeof(List<MelonMod>));

        if (modsField == null)
        {
            Main.Logger.Warning("Could not get MelonHandler Mods field! The plugin will fail.");
            return;
        }

        try
        {
            var thisAsm = typeof(DummyMod).Assembly;

            Mod = new DummyMod();
            SetProp(nameof(Mod.Assembly), thisAsm);
            SetProp(nameof(Mod.Info), thisAsm.GetCustomAttribute<MelonInfoAttribute>());
            SetProp(nameof(Mod.Games), new[] {thisAsm.GetCustomAttribute<MelonGameAttribute>()});
            SetProp(nameof(Mod.ConsoleColor), BuildInfo.MelonColor);
            SetProp(nameof(Mod.AuthorConsoleColor), MelonLogger.DefaultTextColor);
            SetProp(nameof(Mod.Location), thisAsm.Location);
            SetProp(nameof(Mod.Priority), int.MinValue);
            SetProp(nameof(Mod.LoggerInstance), Main.Logger);
            SetProp(nameof(Mod.HarmonyInstance), Main.Harmony);

            var mods = (List<MelonMod>) modsField.GetValue(null);
            mods.Add(Mod);
            modsField.SetValue(null, mods);
        }
        catch (Exception e)
        { Main.Logger.Warning($"Could not inject into MelonHandler Mods! The plugin will fail. Error: {e}"); }
        
        Main.Logger.Msg("MelonMod injected successfully!");
    }

    private static void SetProp<T>(string propName, T val)
    {
        PropertyInfo propInfo = null;
        var type = Mod.GetType();
        while (propInfo == null && type != null)
        {
            propInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            type = type.BaseType;
        }
        if (propInfo == null)
            throw new ArgumentOutOfRangeException(nameof(propName),
                $"Field {propName} was not found in Type {Mod.GetType().FullName}");
        
        propInfo.SetValue(Mod, val, null);
    }
}