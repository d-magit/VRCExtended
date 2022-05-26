using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;

namespace VRCExtended.Management;

internal static class MelonModDetour
{
    internal static VRCExtendedMod ModInstance;
    
    internal static void InjectMelon()
    {
        var modsField = typeof(MelonHandler).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .FirstOrDefault(p => p.FieldType == typeof(List<MelonMod>));

        if (modsField == null)
        {
            VRCExtendedPlugin.Logger.Warning("Could not inject into MelonHandler.Mods! The plugin will fail.");
            return;
        }

        try
        {
            var thisAsm = typeof(VRCExtendedMod).Assembly;

            ModInstance = new VRCExtendedMod();
            SetPrivateProp(ModInstance, nameof(ModInstance.Assembly), thisAsm);
            SetPrivateProp(ModInstance, nameof(ModInstance.Info), thisAsm.GetCustomAttribute<MelonInfoAttribute>());
            SetPrivateProp(ModInstance, nameof(ModInstance.Games), new[] {thisAsm.GetCustomAttribute<MelonGameAttribute>()});
            SetPrivateProp(ModInstance, nameof(ModInstance.ConsoleColor), BuildInfo.MelonColor);
            SetPrivateProp(ModInstance, nameof(ModInstance.AuthorConsoleColor), MelonLogger.DefaultTextColor);
            SetPrivateProp(ModInstance, nameof(ModInstance.Location), thisAsm.Location);
            SetPrivateProp(ModInstance, nameof(ModInstance.Priority), int.MinValue);
            SetPrivateProp(ModInstance, nameof(ModInstance.LoggerInstance), VRCExtendedPlugin.Logger);
            SetPrivateProp(ModInstance, nameof(ModInstance.HarmonyInstance), VRCExtendedPlugin.Harmony);

            var mods = (List<MelonMod>) modsField.GetValue(null);
            mods.Add(ModInstance);
            modsField.SetValue(null, mods);
        }
        catch (Exception e)
        { VRCExtendedPlugin.Logger.Warning($"Could not inject into MelonHandler.Mods! The plugin will fail. Error: {e}"); }
    }

    private static void SetPrivateProp<T>(object obj, string propName, T val)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        
        PropertyInfo propInfo = null;
        var type = obj.GetType();
        while (propInfo == null && type != null)
        {
            propInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            type = type.BaseType;
        }
        if (propInfo == null)
            throw new ArgumentOutOfRangeException(nameof(propName),
                $"Field {propName} was not found in Type {obj.GetType().FullName}");
        
        propInfo.SetValue(obj, val, null);
    }
}