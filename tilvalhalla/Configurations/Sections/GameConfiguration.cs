using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class GameConfiguration
    {

        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<string> firstspawnmessage;
        public static ConfigEntry<bool> DisableBuildRestrictions;

        public static void Awake(BaseUnityPlugin gamecfg)
        {
            enabled = gamecfg.Config.Bind("Game", "enabled", false, new ConfigDescription("Set this to true if you want game configuration section enabled", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            firstspawnmessage = gamecfg.Config.Bind("Game", "firstspawnmessage", "I have arrived!" , new ConfigDescription("This is the message that displays when a player has arrived", null));
            DisableBuildRestrictions = gamecfg.Config.Bind("Game", "DisableBuildRestrictoins", false, new ConfigDescription("Set this to true to disable build restrictions on spawn", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }

    }



}