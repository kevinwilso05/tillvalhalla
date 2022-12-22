using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class containerconfiguration
    {
        public static ConfigEntry<bool> enabled; 
        public static ConfigEntry<int> woodChestInventoryRows;
        public static ConfigEntry<int> woodChestInventoryCol;
        public static ConfigEntry<int> ironChestRows;
        public static ConfigEntry<int> ironChestCol;
        public static ConfigEntry<int> blackMetalChestRows;
        public static ConfigEntry<int> blackMetalChestCol; 
        public static ConfigEntry<int> privateChestRows;
        public static ConfigEntry<int> privateChestCol;

        public static void Awake(BaseUnityPlugin containercfg)
        {
            containercfg.Config.SaveOnConfigSet = true;

            enabled = containercfg.Config.Bind("Container", "enabled", false, new ConfigDescription("Set this to true to enable this section for container", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            woodChestInventoryRows = containercfg.Config.Bind("Container", "Wood Chest Rows", 5, new ConfigDescription("This setting affects the rows in wood chests", new AcceptableValueRange<int>(3, 8), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            woodChestInventoryCol = containercfg.Config.Bind("Container", "Wood Chest Columns", 2, new ConfigDescription("This setting affects the columns in wood chests", new AcceptableValueRange<int>(2, 10), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            ironChestRows = containercfg.Config.Bind("Container", "Iron Chest Rows", 7, new ConfigDescription("This setting affects the rows in iron chests", new AcceptableValueRange<int>(3, 8), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            ironChestCol = containercfg.Config.Bind("Container", "Iron Chest Columns", 4, new ConfigDescription("This setting affects the columns in iron chests", new AcceptableValueRange<int>(3, 20), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            blackMetalChestRows = containercfg.Config.Bind("Container", "Black Metal Chest Rows", 7, new ConfigDescription("This setting affects the rows in black metal chests", new AcceptableValueRange<int>(3, 8), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            blackMetalChestCol = containercfg.Config.Bind("Container", "Black Metal Chest Columns", 4, new ConfigDescription("This setting affects the columns in black metal chests", new AcceptableValueRange<int>(3, 20), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            privateChestRows = containercfg.Config.Bind("Container", "Private Chest Rows", 4, new ConfigDescription("This setting affects the rows in private chests", new AcceptableValueRange<int>(3, 8), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            privateChestCol = containercfg.Config.Bind("Container", "Private Chest Columns", 6, new ConfigDescription("This setting affects the columns in private chests", new AcceptableValueRange<int>(2, 20), new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }
    }
}