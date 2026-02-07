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
            enableDebugLogging = cfg.Config.Bind("General", "Enable Debug Logging", false, new ConfigDescription("Set this to true to enable debug logging.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }
    }

}