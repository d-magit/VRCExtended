using System;
using MelonLoader;
using VRCExtended.Mod;

namespace VRCExtended;

public static partial class EventSubscriber
{
	private static void SubscribeCustomEvents(MelonBase melon)
	{
		var melonName = melon.Info.Name;
		if (melon is IOnUiManagerInit umi)
			Detour.Mod.UiManagerInit += () => {
				try
				{ umi.OnUiManagerInit(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(umi.OnUiManagerInit)); }
			};
		if (melon is IOnPlayerJoined pj)
			Detour.Mod.PlayerJoined += player => {
				try
				{ pj.OnPlayerJoined(player); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(pj.OnPlayerJoined)); }
			};
		if (melon is IOnPlayerLeft pl)
			Detour.Mod.PlayerLeft += player => {
				try
				{ pl.OnPlayerLeft(player); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(pl.OnPlayerLeft)); }
			};
		if (melon is IOnInstanceChanged ic)
			Detour.Mod.InstanceChanged += (apiWorld, apiWorldInstance) => {
				try
				{ ic.OnInstanceChanged(apiWorld, apiWorldInstance); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(ic.OnInstanceChanged)); }
			};
		if (melon is IOnUnload mu)
			Detour.Mod.Unload += () => {
				try
				{ mu.OnUnload(); }
				catch (Exception e)
				{ LogInternalError(e, melonName, nameof(mu.OnUnload)); }
			};
	}
}