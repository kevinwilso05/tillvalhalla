using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class inventoryconfiguration
    {
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<int> playerinventoryrows;
        public static ConfigEntry<bool> toptobottomfill;
        public static ConfigEntry<float> maxstacksizemultiplier;
		public static ConfigEntry<int> rowsToSwitch;
		public static ConfigEntry<string> hotKey;

		public static void Awake(BaseUnityPlugin inventorycfg)
        {

            enabled = inventorycfg.Config.Bind("Inventory", "enabled", false, new ConfigDescription("Set this to true to enable the inventory section", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            playerinventoryrows = inventorycfg.Config.Bind("Inventory", "playerinventoryrows", 4, new ConfigDescription("Determines the amount of inventory rows", new AcceptableValueRange<int>(4,20), new ConfigurationManagerAttributes { IsAdminOnly = true }));
            toptobottomfill = inventorycfg.Config.Bind("Inventory", "toptobottomfill", true , new ConfigDescription("Set this to true to enable the inventory to fill from the top to the bototm",null , new ConfigurationManagerAttributes { IsAdminOnly = true }));
            maxstacksizemultiplier = inventorycfg.Config.Bind("Inventory", "maxstacksizemultiplier", 0f, new ConfigDescription("Determines the stack size of items in a % multiplier", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            rowsToSwitch = inventorycfg.Config.Bind<int>("General", "RowsToSwitch", 2, "Rows of inventory to switch via hotkey");
			hotKey = inventorycfg.Config.Bind<string>("General", "HotKey", "`", "Hotkey to initiate switch.");
		}
    }
}