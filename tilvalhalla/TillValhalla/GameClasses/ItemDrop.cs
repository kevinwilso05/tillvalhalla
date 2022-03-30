using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;


namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class ItemDrop_Awake_Patch
    {
        public static void Postfix(ItemDrop __instance)
        {
            if (ItemDropConfiguration.noteleportprevention.Value && GameConfiguration.isenabled.Value)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

        if (GameConfiguration.isenabled.Value) //Check if Mod is Enabled
        {
            //Check type on item and set movement modifier on equip to 0
        var itemtype = __instance.m_itemData.m_shared.m_itemType.ToString();


           //All items movement speed modifier
                __instance.m_itemData.m_shared.m_movementModifier = ItemDropConfiguration.movementmodifier.Value;
            
        }
        

    }
    }

    
}