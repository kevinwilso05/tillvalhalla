using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class gatherconfiguration
    {
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<float> wood;
        public static ConfigEntry<float> stone;
        public static ConfigEntry<float> fineWood;
        public static ConfigEntry<float> coreWood;
        public static ConfigEntry<float> elderBark;
        public static ConfigEntry<float> ironScrap;
        public static ConfigEntry<float> tinOre;
        public static ConfigEntry<float> copperOre;
        public static ConfigEntry<float> silverOre;
        public static ConfigEntry<float> chitin;
        public static ConfigEntry<float> Dropchance;
        public static ConfigEntry<float> greydwarfeye;

        public static void Awake(BaseUnityPlugin gathercfg)
        {

            enabled = gathercfg.Config.Bind("Gather", "enabled", false, new ConfigDescription("Set this to true to enable the sather section", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Dropchance = gathercfg.Config.Bind("Gather", "Dropchance", 0f, new ConfigDescription("As example by default scrap piles in dungeons have a 20% chance to drop a item, if you set this option to 200, you will then have a 60% chance to drop iron.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            wood = gathercfg.Config.Bind("Gather", "wood", 0f, new ConfigDescription("Determines the drop rate of wood in %. For example, if the value is set to 50, then 10 pieces of wood would now drop as 15. This can be negative.",null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            fineWood = gathercfg.Config.Bind("Gather", "fineWood", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            coreWood = gathercfg.Config.Bind("Gather", "coreWood", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            elderBark = gathercfg.Config.Bind("Gather", "elderBark", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            ironScrap = gathercfg.Config.Bind("Gather", "ironScrap", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            tinOre = gathercfg.Config.Bind("Gather", "tinOre", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            copperOre = gathercfg.Config.Bind("Gather", "copperOre", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            silverOre = gathercfg.Config.Bind("Gather", "silverOre", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            chitin = gathercfg.Config.Bind("Gather", "chitin", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            stone = gathercfg.Config.Bind("Gather", "stone", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            greydwarfeye = gathercfg.Config.Bind("Gather", "greydwarfeye", 0f, new ConfigDescription("test", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));


        }
    }
}