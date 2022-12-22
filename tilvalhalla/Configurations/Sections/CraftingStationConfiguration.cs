using BepInEx;
using BepInEx.Configuration;
using Steamworks;
using System.Security.Policy;

namespace TillValhalla.Configurations.Sections
{
    public class CraftingStationConfiguration
    {
        public static ConfigEntry<bool> craftingroofrequired;
        public static ConfigEntry<bool> craftFromCarts;
        public static ConfigEntry<bool> craftFromShips;
        public static ConfigEntry<bool> craftFromChests;
        public static ConfigEntry<bool> craftFromWorkbench;
        public static ConfigEntry<bool> ignorePrivateAreaCheck;
        public static ConfigEntry<float> craftFromChestRange;


        public static void Awake(BaseUnityPlugin craftingcfg)
        {

            craftingroofrequired = craftingcfg.Config.Bind("Crafting", "craftingroofrequired", true, new ConfigDescription("Set this to false to disable crafting stations needing a roof.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            craftFromCarts = craftingcfg.Config.Bind("Crafting", "Craft from Carts", false, new ConfigDescription("Set this to true to enable crafting from carts.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            craftFromShips = craftingcfg.Config.Bind("Crafting", "Craft from Ships", false, new ConfigDescription("Set this to true to enable crafting from ships.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            craftFromChests = craftingcfg.Config.Bind("Crafting", "Craft from Chests", false, new ConfigDescription("Set this to true to enable crafting from Chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            craftFromWorkbench = craftingcfg.Config.Bind("Crafting", "Craft from Workbench", false, new ConfigDescription("If in a workbench area, use it as a reference point when scanning for chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            ignorePrivateAreaCheck = craftingcfg.Config.Bind("Crafting", "Ignore Private Area Check", false, new ConfigDescription("Set this to true to allow crafting from warded areas", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            craftFromChestRange = craftingcfg.Config.Bind("Crafting", "Craft from Chest Range", 50f, new ConfigDescription("Range of the detection of chests.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }
    }
}