using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class ShipConfiguration
    {

        
        public static ConfigEntry<float> sailforcefactor;
        public static ConfigEntry<float> rudderspeed;
        public static ConfigEntry<float> stearforce;
        public static ConfigEntry<bool> shipFireRested;

        public static void Awake(BaseUnityPlugin shipcfg)
        {
            shipcfg.Config.SaveOnConfigSet = true;

            sailforcefactor = shipcfg.Config.Bind("Ship", "sailforcefactor", 0.03f , new ConfigDescription("Change this to change the ship speed.", new AcceptableValueRange<float>(0.03f,0.5f), new ConfigurationManagerAttributes { IsAdminOnly = true }));

            rudderspeed = shipcfg.Config.Bind("Ship", "rudderspeed", 1f, new ConfigDescription("Change this to modify how fast the rudder turns", new AcceptableValueRange<float>(1, 10), new ConfigurationManagerAttributes { IsAdminOnly = true }));

            stearforce = shipcfg.Config.Bind("Ship", "stearforce", 0.2f , new ConfigDescription("Change this to modify how much force the stearing exerts", new AcceptableValueRange<float>(0.2f,1f), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            shipFireRested = shipcfg.Config.Bind("Ship", "shipFireRested", true, new ConfigDescription("Set to true to enable the firepit on the ship to give rested bonus.", null, new ConfigurationManagerAttributes { IsAdminOnly = false }));

        }

    }



}