using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class PlayerConfiguration
    {

        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<float> basemaximumweight;
        public static ConfigEntry<float> basestamina;
        public static ConfigEntry<float> basehp;
        public static ConfigEntry<float> baseMegingjordBuff;
        public static ConfigEntry<bool> autorepair;
        

        public static void Awake(BaseUnityPlugin playercfg)
        {
            //playercfg.Config.SaveOnConfigSet = true;
            enabled = playercfg.Config.Bind("Player", "enabled", false, new ConfigDescription("Set this to true if you want the player configuration section enabled", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            basemaximumweight = playercfg.Config.Bind("Player", "basemaximumweight", 300f, new ConfigDescription("This is the base amount of maxmimum weight", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            basestamina = playercfg.Config.Bind("Player", "basestamina", 75f, new ConfigDescription("Base amount of stamina", null , new ConfigurationManagerAttributes { IsAdminOnly = true }));
            basehp = playercfg.Config.Bind("Player", "basehp", 25f, new ConfigDescription("Base amount of HP", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            baseMegingjordBuff = playercfg.Config.Bind("Player", "baseMegingjordBuff", 200f, new ConfigDescription("Base Megingjord Buff", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            autorepair = playercfg.Config.Bind("Player", "auto repair", false, new ConfigDescription("Set this to true if you want to auto repair items when interacting with workbench", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

    }



}