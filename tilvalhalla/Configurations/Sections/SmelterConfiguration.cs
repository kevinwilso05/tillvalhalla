using BepInEx;
using BepInEx.Configuration;
using Steamworks;
using System.Security.Policy;

namespace TillValhalla.Configurations.Sections
{
    public class SmelterConfiguration
    {
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<bool> kilsIsEnabled;
        public static ConfigEntry<int> kilnMaxWood;
        public static ConfigEntry<int> kilnSpeed;
        public static ConfigEntry<bool> smelterIsEnabled;
        public static ConfigEntry<int> smelterMaxOre;
        public static ConfigEntry<int> smelterMaxCoal;
        public static ConfigEntry<int> smelterSpeed;
        public static ConfigEntry<int> smelterCoalUsedPerProduct;
        public static ConfigEntry<bool> furnaceIsEnabled;
        public static ConfigEntry<int> furnaceMaxOre;
        public static ConfigEntry<int> furnaceMaxCoal;
        public static ConfigEntry<int> furnaceSpeed;
        public static ConfigEntry<int> furnaceCoalUsedPerProduct;
        public static ConfigEntry<bool> windmillIsEnabled;
        public static ConfigEntry<int> windmillMaxBarley;
        public static ConfigEntry<int> windmillProductionSpeed;
        public static ConfigEntry<bool> spinningWheelIsEnabled;
        public static ConfigEntry<int> spinningWheelMaxFlax;
        public static ConfigEntry<int> spinningWheelProductionSpeed;


        public static void Awake(BaseUnityPlugin smeltcfg)
        {

            kilsIsEnabled = smeltcfg.Config.Bind("Kiln", "Enabled", false, new ConfigDescription("Set this to true to enable the kiln section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            kilnMaxWood = smeltcfg.Config.Bind("Kiln", "Kiln Max Wood", 25, new ConfigDescription("The max amount of wood a kiln can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            kilnSpeed = smeltcfg.Config.Bind("Kiln", "Kiln production speed", 15, new ConfigDescription("The production speed of the kiln in seconds to produce a piece of coal.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            smelterIsEnabled = smeltcfg.Config.Bind("Smelter", "Enabled", false, new ConfigDescription("Set this to true to enable the smelter section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            smelterMaxOre = smeltcfg.Config.Bind("Smelter", "Smelter Max Ore", 10, new ConfigDescription("The max amount of ore a smelter can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            smelterMaxCoal = smeltcfg.Config.Bind("Smelter", "Smelter Max Coal", 20, new ConfigDescription("The max amount of coal a smelter can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            smelterSpeed = smeltcfg.Config.Bind("Smelter", "Smelter production speed", 30, new ConfigDescription("Production speed of the smelter in seconds to produce an ingot", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            smelterCoalUsedPerProduct  = smeltcfg.Config.Bind("Smelter", "Smelter Coal per ingot", 2, new ConfigDescription("Amount of coal needed to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

            furnaceIsEnabled = smeltcfg.Config.Bind("Furnace", "Enabled", false, new ConfigDescription("Set this to true to enable the furnace section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            furnaceMaxOre = smeltcfg.Config.Bind("Furnace", "Furnace Max Ore", 10, new ConfigDescription("The max amount of ore in a furnace.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            furnaceMaxCoal = smeltcfg.Config.Bind("Furnace", "Furnace Max Coal", 25, new ConfigDescription("The max amount of coal a furnace can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            furnaceSpeed = smeltcfg.Config.Bind("Furnace", "Furnace production speed", 30, new ConfigDescription("Production speed of the furnace in seconds to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            furnaceCoalUsedPerProduct = smeltcfg.Config.Bind("Furnace", "Furnace Coal per Ingot", 2, new ConfigDescription("Amount of coal needed to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

            windmillIsEnabled = smeltcfg.Config.Bind("Windmill", "Enabled", false, new ConfigDescription("Set this to true to enable the windmill section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            windmillMaxBarley = smeltcfg.Config.Bind("Windmill", "Windmill Max Barley", 100, new ConfigDescription("Max amount of barley in a windmill.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            windmillProductionSpeed = smeltcfg.Config.Bind("Windmill", "Windmill production speed", 10, new ConfigDescription("Production speed of the windmill in seconds to produce flour.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

            spinningWheelIsEnabled = smeltcfg.Config.Bind("SpinningWheel", "Enabled", false, new ConfigDescription("Set this to true to enable the spinning wheel section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            spinningWheelMaxFlax = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel Max Flax", 100, new ConfigDescription("Max amount of flax in a windmill.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            spinningWheelProductionSpeed = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel production speed", 10, new ConfigDescription("Production speed of the spinning wheel in seconds to produce one item", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }
    }
}