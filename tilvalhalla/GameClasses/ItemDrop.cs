using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;
using System;
using System.Text;
using UnityEngine;


namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class ItemDrop_Awake_Patch
    {
        public static void Postfix(ItemDrop __instance)
        {
            if (ItemDropConfiguration.noteleportprevention.Value && Configuration.modisenabled.Value)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

            if (Configuration.modisenabled.Value) //Check if Mod is Enabled
            {
                //Check type on item and set movement modifier on equip to 0
                var itemtype = __instance.m_itemData.m_shared.m_itemType.ToString();
                int maxstack = __instance.m_itemData.m_shared.m_maxStackSize;
                float maxstackmodified = helper.applyModifierValue(maxstack, inventoryconfiguration.maxstacksizemultiplier.Value);
                itemtype = __instance.m_itemData.m_shared.m_itemType.ToString();
                if (itemtype == "Consumable" || itemtype == "Material" || itemtype == "Ammo")
                {
                    __instance.m_itemData.m_shared.m_maxStackSize = (int)Math.Round(maxstackmodified);
                }
                //All items movement speed modifier
                if (ItemDropConfiguration.movementmodifier.Value == 0f)
                {
                    __instance.m_itemData.m_shared.m_movementModifier = ItemDropConfiguration.movementmodifier.Value;
                }
                else
                {
                    if (__instance.m_itemData.m_shared.m_movementModifier != 0f)
                    {
                        float originalValue = __instance.m_itemData.m_shared.m_movementModifier;
                        __instance.m_itemData.m_shared.m_movementModifier = ItemDropConfiguration.movementmodifier.Value;
                    }
                }
            }
        }
    }
}