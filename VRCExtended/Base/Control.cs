using System;
using System.Reflection;
using MelonLoader;

namespace VRCExtended.Base;

public abstract partial class VrcMod
{
	protected VrcMod()
	{
		RegisterMelonEvents();
		RegisterCustomEvents();
	}
	
	private bool IsOverriding(string methodName)
	{
		var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)?.Attributes;
		return method != null && !method.HasFlag(MethodAttributes.NewSlot) && method.HasFlag(MethodAttributes.Virtual);
	}
	
	private void LogInternalError(Exception e, string methodName) => 
		VRCExtendedPlugin.Logger.Error($"Something went wrong in {GetType().Name} during {methodName} execution. Exception: {e}");
}