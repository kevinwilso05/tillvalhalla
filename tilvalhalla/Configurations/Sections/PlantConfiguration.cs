using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class PlantConfiguration
    {

       // public static ConfigEntry<bool> enabled;
        public static ConfigEntry<bool> needgrowspace;
        public static ConfigEntry<bool> needcultivatedground;

        public static void Awake(BaseUnityPlugin gamecfg)
        {
            //enabled = gamecfg.Config.Bind("Plant", "enabled", false, new ConfigDescription("Set this to true if you want game configuration section enabled", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            needgrowspace = gamecfg.Config.Bind("Plant", "needgrowspace", true, new ConfigDescription("Set this to false to allow planting without grow space", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            needcultivatedground = gamecfg.Config.Bind("Plant", "needcultivatedground", true, new ConfigDescription("Set this to false to allow planting without cultivated ground", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }

    }
}


