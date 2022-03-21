using BepInEx;
using BepInEx.Configuration;

namespace TillValhalla.Configurations.Sections
{
    public class ItemDropConfiguration
    {
        
        public static ConfigEntry<bool> noteleportprevention;
        public static ConfigEntry<float> movementmodifier;

        public static void Awake(BaseUnityPlugin itemdropcfg)
        {
            
            noteleportprevention = itemdropcfg.Config.Bind("ItemDrop", "noteleportprevention", false, "Set this to true to turn off teleport prevention");
            movementmodifier = itemdropcfg.Config.Bind("ItemDrop", "movementmodifier", -.05f, "Modifies the movement speed for equiped armor");
        }
    }

}