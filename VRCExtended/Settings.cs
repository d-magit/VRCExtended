using VRCExtended.Utils;
using System;
using System.IO;
using Newtonsoft.Json;

namespace VRCExtended;

internal static class Settings
{
    private static string _configPath;
    private static Config _instance;
    internal static Config Instance
    {
        get => _instance ??= LoadConfig();
        set
        {
            _instance = value;
            SaveConfig(value, true);
        }
    }
    
    private static Config LoadConfig()
    {
        var @return = new Config();
            
        // Attempts to load config from the mod folder. If it doesn't exist, it will load default values.
        if (!Utilities.EnsureFolderExists(Utilities.MainFolder))
            return @return;
        
        _configPath = Path.Combine(Utilities.MainFolder, "config.json");
        if (File.Exists(_configPath))
        {
            try
            { @return = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_configPath)); }
            catch
            { VRCExtended.Logger.Warning($"Failed to load config from {_configPath}, loading defaults."); }
            return @return;
        }

        // If config didn't exist, it will save default values to file.
        VRCExtended.Logger.Warning($"No config was found. Loading defaults and creating a new one at {_configPath}.");
        SaveConfig(@return);
        
        return @return;
    }

    private static void SaveConfig() => SaveConfig(Instance, true);
    private static void SaveConfig(Config config, bool logError = false)
    {
        try
        {
            if (!Utilities.EnsureFolderExists(Utilities.MainFolder))
                throw new DirectoryNotFoundException("Mod folder doesn't exist, and failed to create.");
            
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
        catch (Exception e)
        {
            if (logError)
                VRCExtended.Logger.Error($"Failed to save config to {_configPath}: {e}");
        }
    }
}