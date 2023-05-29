using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class SapCollectorConfiguration
    {
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<int> rootmaxcapacity;
        public static ConfigEntry<float> rootregenpersec;
        public static ConfigEntry<float> collectorsecperunit;
        public static ConfigEntry<int> collectormaxcapacity;

        public static void Awake(BaseUnityPlugin sapcollectorcfg)
        {
            sapcollectorcfg.Config.SaveOnConfigSet = true;

            enabled = sapcollectorcfg.Config.Bind("Sap", "Enabled", false, new ConfigDescription("Set this to true to enable this section for sap collectors", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            rootmaxcapacity = sapcollectorcfg.Config.Bind("Sap", "Root Max Capacity", 50, new ConfigDescription("This setting affects the max capacity of roots", new AcceptableValueRange<int>(30, 300), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            rootregenpersec = sapcollectorcfg.Config.Bind("Sap", "Sap Regen per Sec", 0.0025f, new ConfigDescription("This setting affects the sap regen rate in roots per second", new AcceptableValueRange<float>(0.0025f, 40), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            collectorsecperunit = sapcollectorcfg.Config.Bind("Sap", "Collector Seconds per Unit", 60f, new ConfigDescription("This setting affects the amount of seconds it takes to get one sap", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            collectormaxcapacity = sapcollectorcfg.Config.Bind("Sap", "Collector Max Capacity", 10, new ConfigDescription("This setting affects the max capacity of the sap collector", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }
    }
}