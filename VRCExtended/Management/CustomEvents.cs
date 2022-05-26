using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using VRC;
using VRC.Core;

namespace VRCExtended.Management;

internal static class CustomEvents
{
    internal static void Init()
    {
        VRCExtendedPlugin.Harmony.Patch(typeof(RoomManager)
                .GetMethod(nameof(RoomManager.Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_String_Int32_0)),
            null, new HarmonyMethod(typeof(CustomEvents).GetMethod(nameof(OnInstanceChangeMethod), BindingFlags.NonPublic | BindingFlags.Static)));
        
        static IEnumerator OnUiManagerInitIEnum()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return null;
            OnUiManagerInit();
        }
        MelonCoroutines.Start(OnUiManagerInitIEnum());
    }
    
    private static void OnInstanceChangeMethod(ApiWorld __0, ApiWorldInstance __1) => MelonModDetour.ModInstance.OnInstanceChanged(__0, __1);

    private static void OnUiManagerInit()
    {
        NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.
            field_Private_HashSet_1_UnityAction_1_T_0.Add(EventHandlerA);
        NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.
            field_Private_HashSet_1_UnityAction_1_T_0.Add(EventHandlerB);
        MelonModDetour.ModInstance.OnUiManagerInit();
    }

    private static Action<Player> _eventHandlerA;
    private static Action<Player> _eventHandlerB;
    private static Action<Player> EventHandlerA
    {
        get
        {
            _eventHandlerB ??= MelonModDetour.ModInstance.OnPlayerLeft;
            return _eventHandlerA ??= MelonModDetour.ModInstance.OnPlayerJoined;
        }
    }
    private static Action<Player> EventHandlerB
    {
        get
        {
            _eventHandlerA ??= MelonModDetour.ModInstance.OnPlayerLeft;
            return _eventHandlerB ??= MelonModDetour.ModInstance.OnPlayerJoined;
        }
    }
}