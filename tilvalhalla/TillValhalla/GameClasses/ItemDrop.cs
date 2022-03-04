using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace TillValhalla.GameClasses
{

    

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class noItemTeleportPrevention
    {
        public static void Postfix(ItemDrop __instance)
        {
            __instance.m_itemData.m_shared.m_teleportable = true;

        }
    }

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class nomovementspeeddecrease
    {
        public static void Postfix(ItemDrop __instance)
        {
            
            //Check type on item and set movement modifier on equip to 0
            var itemtype = __instance.m_itemData.m_shared.m_itemType.ToString();
            

            if (itemtype == "Chest" || itemtype == "Legs" || itemtype == "Helmet")
            {
                __instance.m_itemData.m_shared.m_movementModifier = 0;
            }

            

        }
    }
}