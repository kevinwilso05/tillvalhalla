//using BepInEx;
//using BepInEx.Configuration;

//namespace TillValhalla.Configurations.Sections
//{
//    public class SmelterConfiguration
//    {
//        public static ConfigEntry<bool> kilnIsEnabled;
//        public static ConfigEntry<int> kilnMaxWood;
//        public static ConfigEntry<int> kilnSpeed;
//        public static ConfigEntry<int> kilnAutoRange;
//        public static ConfigEntry<bool> kilnAutoDeposit;
//        public static ConfigEntry<bool> dontProcessFineWood;
//        public static ConfigEntry<bool> dontProcessRoundLog;
//        public static ConfigEntry<bool> kilnAutoFuel;
//        public static ConfigEntry<int> kilnStopAutoFuelThreshold;
//        public static ConfigEntry<bool> smelterIsEnabled;
//        public static ConfigEntry<int> smelterMaxOre;
//        public static ConfigEntry<int> smelterMaxCoal;
//        public static ConfigEntry<int> smelterSpeed;
//        public static ConfigEntry<int> smelterCoalUsedPerProduct;
//        public static ConfigEntry<int> smelterAutoRange;
//        public static ConfigEntry<bool> smelterAutoDeposit;
//        public static ConfigEntry<bool> smelterAutoFuel;
//        public static ConfigEntry<bool> furnaceIsEnabled;
//        public static ConfigEntry<int> furnaceMaxOre;
//        public static ConfigEntry<int> furnaceMaxCoal;
//        public static ConfigEntry<int> furnaceSpeed;
//        public static ConfigEntry<int> furnaceCoalUsedPerProduct;
//        public static ConfigEntry<int> furnaceAutoRange;
//        public static ConfigEntry<bool> furnaceAutoDeposit;
//        public static ConfigEntry<bool> furnaceAutoFuel;
//        public static ConfigEntry<bool> windmillIsEnabled;
//        public static ConfigEntry<int> windmillMaxBarley;
//        public static ConfigEntry<int> windmillProductionSpeed;
//        public static ConfigEntry<int> windmillAutoRange;
//        public static ConfigEntry<bool> windmillAutoDeposit;
//        public static ConfigEntry<bool> windmillAutoFuel;
//        public static ConfigEntry<bool> spinningWheelIsEnabled;
//        public static ConfigEntry<int> spinningWheelMaxFlax;
//        public static ConfigEntry<int> spinningWheelProductionSpeed;
//        public static ConfigEntry<int> spinningWheelAutoRange;
//        public static ConfigEntry<bool> spinningWheelAutoDeposit;
//        public static ConfigEntry<bool> spinningWheelAutoFuel;
//        public static ConfigEntry<bool> eitrRefineryIsEnabled;
//        public static ConfigEntry<int> eitrRefineryMaxOre;
//        public static ConfigEntry<int> eitrRefineryMaxCoal;
//        public static ConfigEntry<int> eitrRefinerySpeed;
//        public static ConfigEntry<int> eitrRefineryCoalUsedPerProduct;
//        public static ConfigEntry<int> eitrRefineryAutoRange;
//        public static ConfigEntry<bool> eitrRefineryAutoDeposit;
//        public static ConfigEntry<bool> eitrRefineryAutoFuel;

//        public static void Awake(BaseUnityPlugin smeltcfg)
//        {
//            smeltcfg.Config.SaveOnConfigSet = true;
//            kilnIsEnabled = smeltcfg.Config.Bind("Kiln", "Enabled", false, new ConfigDescription("Set this to true to enable the kiln section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnMaxWood = smeltcfg.Config.Bind("Kiln", "Kiln Max Wood", 25, new ConfigDescription("The max amount of wood a kiln can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnSpeed = smeltcfg.Config.Bind("Kiln", "Kiln production speed", 15, new ConfigDescription("The production speed of the kiln in seconds to produce a piece of coal.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            dontProcessFineWood = smeltcfg.Config.Bind("Kiln", "Dont Process finewood", true, new ConfigDescription("Set this to false allow processing of finewood.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            dontProcessRoundLog = smeltcfg.Config.Bind("Kiln", "Dont Process core wood", true, new ConfigDescription("Set this to false allow processing of core wood.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnAutoRange = smeltcfg.Config.Bind("Kiln", "Kiln Auto range", 20, new ConfigDescription("Range of the kiln to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnAutoDeposit = smeltcfg.Config.Bind("Kiln", "Kiln Auto deposit", false, new ConfigDescription("Deposits items from the kiln into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnAutoFuel = smeltcfg.Config.Bind("Kiln", "Kiln Auto fuel", false, new ConfigDescription("Auto fills the kiln from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            kilnStopAutoFuelThreshold = smeltcfg.Config.Bind("Kiln", "Kiln auto stop threshold", 300, new ConfigDescription("Amount of coal in nearby chests to stop auto production.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

//            smelterIsEnabled = smeltcfg.Config.Bind("Smelter", "Enabled", false, new ConfigDescription("Set this to true to enable the smelter section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterMaxOre = smeltcfg.Config.Bind("Smelter", "Smelter Max Ore", 10, new ConfigDescription("The max amount of ore a smelter can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterMaxCoal = smeltcfg.Config.Bind("Smelter", "Smelter Max Coal", 20, new ConfigDescription("The max amount of coal a smelter can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterSpeed = smeltcfg.Config.Bind("Smelter", "Smelter production speed", 30, new ConfigDescription("Production speed of the smelter in seconds to produce an ingot", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterCoalUsedPerProduct = smeltcfg.Config.Bind("Smelter", "Smelter Coal per ingot", 2, new ConfigDescription("Amount of coal needed to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterAutoRange = smeltcfg.Config.Bind("Smelter", "Smelter Auto range", 20, new ConfigDescription("Range of the smelter to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterAutoDeposit = smeltcfg.Config.Bind("Smelter", "Smelter Auto deposit", false, new ConfigDescription("Deposits items from the smelter into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            smelterAutoFuel = smeltcfg.Config.Bind("Smelter", "Smelter Auto fuel", false, new ConfigDescription("Auto fills the smelter from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

//            furnaceIsEnabled = smeltcfg.Config.Bind("Furnace", "Enabled", false, new ConfigDescription("Set this to true to enable the furnace section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceMaxOre = smeltcfg.Config.Bind("Furnace", "Furnace Max Ore", 10, new ConfigDescription("The max amount of ore in a furnace.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceMaxCoal = smeltcfg.Config.Bind("Furnace", "Furnace Max Coal", 25, new ConfigDescription("The max amount of coal a furnace can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceSpeed = smeltcfg.Config.Bind("Furnace", "Furnace production speed", 30, new ConfigDescription("Production speed of the furnace in seconds to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceCoalUsedPerProduct = smeltcfg.Config.Bind("Furnace", "Furnace Coal per Ingot", 2, new ConfigDescription("Amount of coal needed to produce an ingot.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceAutoRange = smeltcfg.Config.Bind("Furnace", "Furnace Auto range", 20, new ConfigDescription("Range of the furnace to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceAutoDeposit = smeltcfg.Config.Bind("Furnace", "Furnace Auto deposit", false, new ConfigDescription("Deposits items from the furnace into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            furnaceAutoFuel = smeltcfg.Config.Bind("Furnace", "Furnace Auto fuel", false, new ConfigDescription("Auto fills the furnace from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

//            windmillIsEnabled = smeltcfg.Config.Bind("Windmill", "Enabled", false, new ConfigDescription("Set this to true to enable the windmill section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            windmillMaxBarley = smeltcfg.Config.Bind("Windmill", "Windmill Max Barley", 100, new ConfigDescription("Max amount of barley in a windmill.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            windmillProductionSpeed = smeltcfg.Config.Bind("Windmill", "Windmill production speed", 10, new ConfigDescription("Production speed of the windmill in seconds to produce flour.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            windmillAutoRange = smeltcfg.Config.Bind("Windmill", "Windmill Auto range", 20, new ConfigDescription("Range of the Windmill to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            windmillAutoDeposit = smeltcfg.Config.Bind("Windmill", "Windmill Auto deposit", false, new ConfigDescription("Deposits items from the Windmill into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            windmillAutoFuel = smeltcfg.Config.Bind("Windmill", "Windmill Auto fuel", false, new ConfigDescription("Auto fills the windmill from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

//            spinningWheelIsEnabled = smeltcfg.Config.Bind("SpinningWheel", "Enabled", false, new ConfigDescription("Set this to true to enable the spinning wheel section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            spinningWheelMaxFlax = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel Max Flax", 100, new ConfigDescription("Max amount of flax in a windmill.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            spinningWheelProductionSpeed = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel production speed", 10, new ConfigDescription("Production speed of the spinning wheel in seconds to produce one item", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            spinningWheelAutoRange = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel Auto range", 20, new ConfigDescription("Range of the Spinning Wheel to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            spinningWheelAutoDeposit = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel Auto deposit", false, new ConfigDescription("Deposits items from the Spinning Wheel into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            spinningWheelAutoFuel = smeltcfg.Config.Bind("SpinningWheel", "Spinning Wheel Auto fuel", false, new ConfigDescription("Autofills the Spinning Wheel from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));


//            eitrRefineryIsEnabled = smeltcfg.Config.Bind("EitrRefinery", "Enabled", false, new ConfigDescription("Set this to true to enable the Eitr Refinery section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryMaxOre = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery Max tissue", 20, new ConfigDescription("The max amount of tissue in a Eitr Refinery.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryMaxCoal = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery Max Sap", 20, new ConfigDescription("The max amount of sap an Eitr Refinery can take.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefinerySpeed = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery production speed", 40, new ConfigDescription("Production speed of the Eitr Refinery in seconds to produce eitr.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryCoalUsedPerProduct = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery sap per eitr", 1, new ConfigDescription("Amount of sap needed to produce a eitr.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryAutoRange = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery Auto range", 20, new ConfigDescription("Range of the Eitr Refinery to pull resources.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryAutoDeposit = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery Auto deposit", false, new ConfigDescription("Deposits items from the Eitr Refinery into nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
//            eitrRefineryAutoFuel = smeltcfg.Config.Bind("EitrRefinery", "Eitr Refinery Auto fuel", false, new ConfigDescription("Autofills the Eitr Refinery from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));


//        }
//    }
//}