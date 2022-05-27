using System;
using VRC;
using VRC.Core;

namespace VRCExtended.Mod;

// ReSharper disable once InconsistentNaming
public partial class DummyMod : IOnUiManagerInit, IOnPlayerJoined, IOnPlayerLeft, IOnInstanceChanged, IOnUnload
{
    // TODO: Add Unity and VRChat UiManagerInits.
    public void OnUiManagerInit() => UiManagerInit?.Invoke();
    public void OnPlayerJoined(Player player) => PlayerJoined?.Invoke(player);
    public void OnPlayerLeft(Player player) => PlayerLeft?.Invoke(player);
    public void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance) => InstanceChanged?.Invoke(world, instance);
    public void OnUnload() => Unload?.Invoke();
    
    internal event Action UiManagerInit;
    internal event Action<Player> PlayerJoined;
    internal event Action<Player> PlayerLeft;
    internal event Action<ApiWorld, ApiWorldInstance> InstanceChanged;
    internal event Action Unload;
}

/// <summary>
/// This supplies interfaces for modules to be notified on custom VRChat events.
/// Will default to this library's implementations.
/// If you don't want to rely on that, and prefer to use your own implementations, avoid implementing these.
/// </summary>
internal interface IOnUiManagerInit
{
    void OnUiManagerInit();
}
internal interface IOnPlayerJoined
{
    void OnPlayerJoined(Player player);
}
internal interface IOnPlayerLeft
{
    void OnPlayerLeft(Player player);
}
internal interface IOnInstanceChanged
{
    void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance);
}

/// <summary>
/// This supplies an interface for modules to be notified when unloaded.
/// You are responsible for the implementation, avoiding and undoing any leftovers, such as components and modifications to the natural game behaviour.
/// </summary>
public interface IOnUnload
{
    void OnUnload();
}