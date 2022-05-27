using System.Collections.Generic;

namespace VRCExtended;

public class Config
{
    public List<ConfigEntry> Modules { get; set; } = new()
    {
        new ConfigEntry
        {
            Name = "Example Self-provided Module",
            AssemblyName = "ExampleSelfProvidedModule",
            Uri = "https://api.example.com/api/v1/VRCModules/ExampleSelfProvidedModule.dll"
        },
        new ConfigEntry
        {
            Provider = "ExampleProvider",
            Name = "Example Module",
            AssemblyName = "ExampleModule",
        }
    };
    
    public Dictionary<string, string> Providers { get; set; } = new()
    {
        { "ExampleProvider", "https://api.example.com/api/v1/VRCModules" }
    };
}

public class ConfigEntry
{
    public string Provider { get; set; } // If null, it's a self-provided module.
    public string Name { get; set; } // Only here for reference, isn't really necessary.
    public string AssemblyName { get; set; }
    public string Uri { get; set; } // Won't be used unless module is self-provided.
}