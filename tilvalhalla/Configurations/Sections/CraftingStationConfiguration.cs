using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class CraftingStationConfiguration
    {
        public static ConfigEntry<bool> craftingroofrequired;


        public static void Awake(BaseUnityPlugin craftingstationcfg)
        {

            craftingroofrequired = craftingstationcfg.Config.Bind("Crafting", "craftingroofrequired", true, new ConfigDescription("Set this to false to disable crafting stations needing a roof.", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
        }
    }
}