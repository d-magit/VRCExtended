using System;
using System.Reflection;
using MelonLoader;
using VRCExtended.Utils;

namespace VRCExtended.Management;

public enum EntryType
{
    Gac,
    Local,
    Remote
}

public class Entry
{
    public EntryType Type { get; set; }
    public string Path { get; set; }
    public Assembly Assembly { get; set; }
    public Version Version { get; set; }
    public int Priority => 
        Utilities.TryGetAssemblyAttribute<MelonPriorityAttribute>(Assembly, out var result) ? result.Priority : 0;
}
