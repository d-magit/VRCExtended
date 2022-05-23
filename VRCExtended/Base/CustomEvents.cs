using System;
using VRC;
using VRC.Core;

namespace VRCExtended.Base;

/// <summary>
/// This supplies an interface for modules to be notified when unloaded.
/// You are responsible for the implementation, avoiding and undoing any leftovers, such as components and modifications to the natural game behaviour.
/// </summary>
public interface IOnModUnload
{
	void OnModUnload();
}

/// <summary>
/// This supplies an interface for modules to be notified on custom VRChat events.
/// Will default to this library's implementations.
/// If you don't want to rely on that, and prefer to use your own implementations, avoid overriding these methods (new keyword recommended).
/// </summary>
public abstract partial class VrcMod : ICustomEvents
{
	public virtual void OnUiManagerInit() { }
	public virtual void OnPlayerJoined(Player player) { }
	public virtual void OnPlayerLeft(Player player) { }
	public virtual void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance) { }
	
	private void RegisterCustomEvents()
	{
		if (IsOverriding(nameof(OnUiManagerInit))) Management.Manager.Instance.UiManagerInit += () =>
		{
			try
			{ OnUiManagerInit(); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnUiManagerInit)); }
		};
		if (IsOverriding(nameof(OnPlayerJoined))) Management.Manager.Instance.PlayerJoined += player =>
		{
			try
			{ OnPlayerJoined(player); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPlayerJoined)); }
		};
		if (IsOverriding(nameof(OnPlayerLeft))) Management.Manager.Instance.PlayerLeft += player =>
		{
			try
			{ OnPlayerLeft(player); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnPlayerLeft)); }
		};
		if (IsOverriding(nameof(OnInstanceChanged))) Management.Manager.Instance.InstanceChanged += (apiWorld, apiWorldInstance) =>
		{
			try
			{ OnInstanceChanged(apiWorld, apiWorldInstance); }
			catch (Exception e)
			{ LogInternalError(e, nameof(OnInstanceChanged)); }
		};
	}
}