using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations
{
    public class GameConfiguration
    {
        public static ConfigEntry<bool> isenabled;
        

        public static void Awake(BaseUnityPlugin gamecfg)
        {
            isenabled = gamecfg.Config.Bind("1. General", "isenabled", true, new ConfigDescription("Set this to true to enable this mod",null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
           
        }
    }

}