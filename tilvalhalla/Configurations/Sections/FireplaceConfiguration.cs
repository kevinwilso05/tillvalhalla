using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
	public class FireplaceConfiguration
	{
		public static ConfigEntry<bool> FireplaceIsEnabled;
		public static ConfigEntry<int> FireplaceAutoRange;
		public static ConfigEntry<bool> FireplaceAutoFuel;
		public static ConfigEntry<bool> FireplaceInfiniteFuel;
		public static ConfigEntry<int> TorchAutoRange; 
		public static ConfigEntry<bool> TorchAutoFuel;
		public static ConfigEntry<bool> TorchInfiniteFuel;
		public static ConfigEntry<int> BrazierAutoRange; 
		public static ConfigEntry<bool> BrazierAutoFuel;
		public static ConfigEntry<bool> BrazierInfiniteFuel;

		public static void Awake(BaseUnityPlugin fireplacecfg)
		{
			fireplacecfg.Config.SaveOnConfigSet = true;
			FireplaceIsEnabled = fireplacecfg.Config.Bind("Fireplace", "Enabled", false, new ConfigDescription("Set this to true to enable the Fireplace section.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			FireplaceAutoRange = fireplacecfg.Config.Bind("Fireplace", "Fireplace Auto range", 20, new ConfigDescription("Range of the Fireplace to pull resources from firewood chests.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
			FireplaceAutoFuel = fireplacecfg.Config.Bind("Fireplace", "Fireplace Auto fuel", false, new ConfigDescription("Auto fills the Fireplace from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			FireplaceInfiniteFuel = fireplacecfg.Config.Bind("Fireplace", "Fireplace Infinite fuel", false, new ConfigDescription("Set this to true to make the Fireplace have infinite fuel.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			TorchAutoRange = fireplacecfg.Config.Bind("Fireplace", "Torch Auto range", 20, new ConfigDescription("Range of the Torch to pull resources from firewood chests.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
			TorchAutoFuel = fireplacecfg.Config.Bind("Fireplace", "Torch Auto fuel", false, new ConfigDescription("Auto fills the Torch from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			TorchInfiniteFuel = fireplacecfg.Config.Bind("Fireplace", "Torch Infinite fuel", false, new ConfigDescription("Set this to true to make the Torch have infinite fuel.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			BrazierAutoRange = fireplacecfg.Config.Bind("Fireplace", "Brazier Auto range", 20, new ConfigDescription("Range of the Brazier to pull resources from firewood chests.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { IsAdminOnly = true }));
			BrazierAutoFuel = fireplacecfg.Config.Bind("Fireplace", "Brazier Auto fuel", false, new ConfigDescription("Auto fills the Brazier from nearby chests.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
			BrazierInfiniteFuel = fireplacecfg.Config.Bind("Fireplace", "Brazier Infinite fuel", false, new ConfigDescription("Set this to true to make the Brazier have infinite fuel.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

		}
	}
}