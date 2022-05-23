using System;
using System.Reflection;
using MelonLoader;

namespace VRCExtended.Management;

public enum ModuleType
{
    Gac,
    Local,
    Remote
}

public class ModuleEntry
{
    public ModuleType Type { get; set; }
    public string Path { get; set; }
    public Assembly Assembly { get; set; }
    public Version Version { get; set; }
    public int Priority
    {
        get
        {
            int? result = null;
            try
            { result = Assembly.GetCustomAttribute<MelonPriorityAttribute>()?.Priority; }
            catch
            { /* ignored */ }
            return result ?? 0;
        }
    }
}
