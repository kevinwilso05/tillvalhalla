using BepInEx;
using BepInEx.Configuration;
using TillValhalla;
using TillValhalla.Configurations.Sections;
using UnityEngine;

namespace TillValhalla.Configurations.Sections
{
    public class BeehiveConfiguration
    {

        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<int> beehivemaxhoney;
        public static ConfigEntry<float> beehiveHoneyProductionSpeed;

        public static void Awake(BaseUnityPlugin Plugin)
        {
            Plugin.Config.SaveOnConfigSet = true;

            enabled = Plugin.Config.Bind("Beehive", "enabled", false, new ConfigDescription("Set this to true if you want this section enabled"));

            beehivemaxhoney = Plugin.Config.Bind("Beehive", "beehivemaxhoney", 1, new ConfigDescription("This is the max amount of honey allowed in beehives", new AcceptableValueRange<int>(1, 100)));

            beehiveHoneyProductionSpeed = Plugin.Config.Bind("Beehive", "beehiveHoneyProductionSpeed", 1200f, new ConfigDescription("Speed of honey production in seconds", new AcceptableValueRange<float>(20f, 20000f)));
        
        
        }

    }



}