using System;
using System.Reflection;
using MelonLoader;

namespace VRCExtended;


public static partial class EventSubscriber
{
    private static Type _currentMelonType;
	
    private static bool IsOverriding(string methodName)
    {
        var method = _currentMelonType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)?.Attributes;
        return method != null && !method.HasFlag(MethodAttributes.NewSlot) && method.HasFlag(MethodAttributes.Virtual);
    }
    
    private static void LogInternalError(Exception e, string melonName, string methodName) => 
        Main.Logger.Error($"Something went wrong in {melonName} during {methodName} execution. Exception: {e}");

    internal static void Subscribe(MelonBase melon)
    {
        SubscribeMelonEvents(melon);
        SubscribeCustomEvents(melon);
    }
}