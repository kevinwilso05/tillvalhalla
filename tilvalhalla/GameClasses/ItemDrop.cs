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
                
                // Handle movement modifier based on configuration
                if (__instance.m_itemData.m_shared.m_movementModifier != 0f)
                {
                    // Option 1: Completely disable all movement modifiers
                    if (ItemDropConfiguration.disableMovementModifier.Value)
                    {
                        __instance.m_itemData.m_shared.m_movementModifier = 0f;
                    }
                    // Option 2: Apply percentage-based modification
                    else if (ItemDropConfiguration.movementmodifier.Value != 0f)
                    {
                        float originalMovementModifier = __instance.m_itemData.m_shared.m_movementModifier;
                        float modifiedMovementModifier = helper.applyModifierValue(originalMovementModifier, ItemDropConfiguration.movementmodifier.Value);
                        __instance.m_itemData.m_shared.m_movementModifier = modifiedMovementModifier;
                    }
                }
            }
        }
    }
}