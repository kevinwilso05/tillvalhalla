using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations
{   //Mod Configuration Options
    public class Configuration
    {
        public static ConfigEntry<bool> modisenabled;
        
        

        public static void Awake(BaseUnityPlugin cfg)
        {
            modisenabled = cfg.Config.Bind("1. General", "isenabled", true, new ConfigDescription("Set this to true to enable this mod",null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
           
        }
    }

}