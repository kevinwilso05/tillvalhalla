using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class ZSFXConfiguration
    {
        public static ConfigEntry<bool> enabled;

        public static ConfigEntry<bool> debugMode;

        public static void Awake(BaseUnityPlugin zsfxcfg)
        {
            zsfxcfg.Config.SaveOnConfigSet = true;
            enabled = zsfxcfg.Config.Bind("ZSFX", "Enabled", defaultValue: false, new ConfigDescription("Set this to true to enable this section for custom sfx", null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            debugMode = zsfxcfg.Config.Bind("ZSFX", "Debug Mode", defaultValue: false, new ConfigDescription("Set this to true to enable debug mode", null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
        }
    }

}
