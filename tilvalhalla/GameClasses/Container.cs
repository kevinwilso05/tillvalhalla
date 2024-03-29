﻿using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;
using UnityEngine;
using System.Runtime.Remoting.Messaging;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(Container), "Awake")]
    public static class Container_Awake_Patch
    {
        private static void Postfix(Container __instance, ref Inventory ___m_inventory)
        {
            //if Mod is enabled and the configuration section is enabled. 
            if (!containerconfiguration.enabled.Value && !Configuration.modisenabled.Value)
            {
                return;
            }
            if (__instance == null || ___m_inventory == null || !__instance.transform.parent)
            {
                if (___m_inventory != null)
                {
                    string name = ___m_inventory.m_name;
                    ref int width = ref ___m_inventory.m_width;
                    ref int height = ref ___m_inventory.m_height;
                    switch (name)
                    {
                        case "$piece_chestprivate":
                            height = helper.Clamp(containerconfiguration.privateChestCol.Value, 2, 20);
                            width = helper.Clamp(containerconfiguration.privateChestRows.Value, 3, 8);
                            break;
                        case "$piece_chestwood":
                            height = helper.Clamp(containerconfiguration.woodChestInventoryCol.Value, 2, 10);
                            width = helper.Clamp(containerconfiguration.woodChestInventoryRows.Value, 3, 8);
                            break;
                        case "$piece_chest":
                            height = helper.Clamp(containerconfiguration.ironChestCol.Value, 3, 20);
                            width = helper.Clamp(containerconfiguration.ironChestRows.Value, 3, 8);
                            break;
                        case "$piece_chestblackmetal":
                            height = helper.Clamp(containerconfiguration.blackMetalChestCol.Value, 3, 20);
                            width = helper.Clamp(containerconfiguration.blackMetalChestRows.Value, 3, 8);
                            break;
                    }
                }
            }
            else
            {
                string name2 = __instance.transform.parent.name;
                string name3 = ___m_inventory.m_name;
                ref int width2 = ref ___m_inventory.m_width;
                ref int height2 = ref ___m_inventory.m_height;
                if (name2.Contains("Karve"))
                {
                    height2 = helper.Clamp(containerconfiguration.karveChestCol.Value, 2, 30);
                    width2 = helper.Clamp(containerconfiguration.karveChestRows.Value, 2, 8);
                }
                else if (name2.Contains("VikingShip"))
                {
                    height2 = helper.Clamp(containerconfiguration.vikingshipChestCol.Value, 3, 30);
                    width2 = helper.Clamp(containerconfiguration.vikingshipChestRows.Value, 6, 8);
                }
                else if (name2.Contains("Cart"))
                {
                    height2 = helper.Clamp(containerconfiguration.cartChestCol.Value, 3, 30);
                    width2 = helper.Clamp(containerconfiguration.cartChestRows.Value, 6, 8);
                }
            }
        }
    }
}