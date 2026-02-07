using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations
{   //Mod Configuration Options
    public class Configuration
    {
        public static ConfigEntry<bool> modisenabled;
        public static ConfigEntry<bool> enableDebugLogging;
        


        public static void Awake(BaseUnityPlugin cfg)
        {
            modisenabled = cfg.Config.Bind("1. General", "isenabled", true, new ConfigDescription("Set this to true to enable this mod",null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            enableDebugLogging = cfg.Config.Bind("1. General", "Enable Debug Logging", false, new ConfigDescription("Set this to true to enable debug logging for smelters and fireplaces.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            // Log the debug setting status on startup
            Jotunn.Logger.LogInfo($"TillValhalla Debug Logging is: {(enableDebugLogging.Value ? "ENABLED" : "DISABLED")}");
        }
    }

}