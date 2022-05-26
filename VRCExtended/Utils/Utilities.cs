﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace VRCExtended.Utils;

internal static class Utilities
{
    internal static readonly string MainFolder = Path.Combine(Environment.CurrentDirectory, BuildInfo.Name);
    internal static readonly string ModulesFolder = Path.Combine(MainFolder, "Modules");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VRCPlayer GetLocalVrcPlayer() => VRCPlayer.field_Internal_Static_VRCPlayer_0;

    public static VRCPlayerApi GetLocalVrcPlayerApi() => Player.prop_Player_0.prop_VRCPlayerApi_0;

    public static APIUser GetLocalAPIUser() => Player.prop_Player_0.field_Private_APIUser_0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Player GetPlayerFromID(string id) => PlayerManager.Method_Public_Static_Player_String_0(id);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Player GetPlayerFromPhotonID(int id) => DelegateMethods.GetPlayerFromPhotonID(id);
        
    public enum WorldSDKVersion { None, SDK2, SDK3 }
    public static WorldSDKVersion GetWorldSDKVersion() => 
        VRC_SceneDescriptor._instance switch
        {
            VRC.SDK3.Components.VRCSceneDescriptor => WorldSDKVersion.SDK3,
            VRCSDK2.VRC_SceneDescriptor => WorldSDKVersion.SDK2,
            _ => WorldSDKVersion.None
        };
    
    internal static bool TryGetAssemblyAttribute<T>(Assembly assembly, out T result) where T : Attribute
    {
        result = null;
        
        try
        { result = assembly.GetCustomAttribute<T>(); }
        catch
        { /* ignored */ }
        
        return result != null;
    }

    public static Transform GetBoneTransform(Player player, HumanBodyBones bone)
    {
        var playerPosition = player.transform;
        var avatarManager = player.prop_VRCPlayer_0.prop_VRCAvatarManager_0;
        if (!avatarManager) return playerPosition;
        var animator = avatarManager.field_Private_Animator_0;
        if (!animator) return playerPosition;
        var boneTransform = animator.GetBoneTransform(bone);
        return boneTransform ? playerPosition : boneTransform;
    }

    public static bool ContainsStr(MethodBase methodBase, string match)
    {
        try
        {
            return XrefScanner.XrefScan(methodBase)
                .Any(instance => instance.Type == XrefType.Global &&
                                 instance.ReadAsObject()?.ToString().IndexOf(match, StringComparison.OrdinalIgnoreCase) >= 0);
        } catch { return false; } 
    }

    public static bool WasUsedBy(MethodBase methodBase, string methodName)
    {
        try
        {
            return XrefScanner.UsedBy(methodBase)
                .Any(instance => instance.TryResolve() != null &&
                                 instance.TryResolve().Name.Equals(methodName, StringComparison.Ordinal));
        } catch { return false; } 
    }
    
    private static readonly Dictionary<string, bool> FolderCache = new();

    public static bool EnsureFolderExists(string folderPath)
    {
        if (FolderCache.TryGetValue(folderPath, out var previousResult))
            return previousResult;

        bool result;
        
        if (Directory.Exists(folderPath))
            result = true;
        else
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                result = true;
            }
            catch
            { result = false; }
        }
        
        FolderCache.Add(folderPath, result);
        return result;
    }
}

internal class EnableDisableListener : MonoBehaviour
{
    public EnableDisableListener(IntPtr obj0) : base(obj0) { }
    
    [method: HideFromIl2Cpp]
    internal event Action OnEnabled;

    [method: HideFromIl2Cpp]
    internal event Action OnDisabled;

    private void OnEnable() => OnEnabled?.Invoke();

    private void OnDisable() => OnDisabled?.Invoke();
}