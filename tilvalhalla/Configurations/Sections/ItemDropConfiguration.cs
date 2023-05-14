using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class ItemDropConfiguration
    {
        
        public static ConfigEntry<bool> noteleportprevention;
        public static ConfigEntry<float> movementmodifier;
        public static ConfigEntry<bool> HideAlwaysDropOneEnabled;
        public static ConfigEntry<float> DeerHide;
        public static ConfigEntry<float> ScaleHide;
        public static ConfigEntry<float> LoxPelt;
        public static ConfigEntry<float> LeatherScraps;
        public static ConfigEntry<float> WolfPelt;

        public static void Awake(BaseUnityPlugin itemdropcfg)
        {
            
            noteleportprevention = itemdropcfg.Config.Bind("ItemDrop", "noteleportprevention", false, new ConfigDescription("Set this to true to turn off teleport prevention", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            movementmodifier = itemdropcfg.Config.Bind("ItemDrop", "movementmodifier", -.05f, new ConfigDescription("Modifies the movement speed for equiped armor", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            HideAlwaysDropOneEnabled = itemdropcfg.Config.Bind("ItemDrop", "HideAlwaysDropOneEnabled", false, new ConfigDescription("Set this to true to force all hide drops to be minimum one", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            DeerHide = itemdropcfg.Config.Bind("ItemDrop", "DeerHide", 0f, new ConfigDescription("Determines the drop rate of deer hide in %.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            ScaleHide = itemdropcfg.Config.Bind("ItemDrop", "ScaleHide", 0f, new ConfigDescription("Determines the drop rate of scale hide in %.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LoxPelt = itemdropcfg.Config.Bind("ItemDrop", "LoxPelt", 0f, new ConfigDescription("Determines the drop rate of lox pelt in %.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LeatherScraps = itemdropcfg.Config.Bind("ItemDrop", "LeatherScraps", 0f, new ConfigDescription("Determines the drop rate of leather scraps in %.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            WolfPelt = itemdropcfg.Config.Bind("ItemDrop", "WolfPelt", 0f, new ConfigDescription("Determines the drop rate of Wolf Pelt in %.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }
    }

}