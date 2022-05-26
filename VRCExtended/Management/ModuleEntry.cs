using System;
using System.Reflection;
using MelonLoader;
using VRCExtended.Utils;

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
    public int Priority => 
        Utilities.TryGetAssemblyAttribute<MelonPriorityAttribute>(Assembly, out var result) ? result.Priority : 0;
}
