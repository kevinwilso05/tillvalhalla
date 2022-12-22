using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TillValhalla.Configurations.Sections;


namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(Inventory), "TopFirst")]
    public static class Inventory_TopFirst_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (inventoryconfiguration.enabled.Value &&
                inventoryconfiguration.toptobottomfill.Value)
            {
                __result = true;
                return false;
            }
            else return true;
        }
    }

    [HarmonyPatch(typeof(Inventory), MethodType.Constructor, new Type[] { typeof(string), typeof(Sprite), typeof(int), typeof(int) })]
    public static class Inventory_Constructor_Patch
    {
        private const int playerInventoryMaxRows = 20;
        private const int playerInventoryMinRows = 4;

        public static void Prefix(string name, ref int w, ref int h)
        {
            if (inventoryconfiguration.enabled.Value)
            {
                // Player inventory
                if (name == "Grave" || name == "Inventory")
                {
                    h = helper.Clamp(inventoryconfiguration.playerinventoryrows.Value, playerInventoryMinRows, playerInventoryMaxRows);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Inventory), "MoveAll")]
    public static class Inventory_MoveAll_Patch
    {
        private static void Prefix(ref Inventory __instance, ref Inventory fromInventory)
        {
            //if (!Configuration.Current.Inventory.IsEnabled || !Configuration.Current.Inventory.mergeWithExistingStacks)
            //{
            //    return;
            //}
            List<ItemDrop.ItemData> list = new List<ItemDrop.ItemData>(fromInventory.GetAllItems());
            foreach (ItemDrop.ItemData item in list)
            {
                if (item.m_shared.m_maxStackSize <= 1)
                {
                    continue;
                }
                foreach (ItemDrop.ItemData item2 in __instance.m_inventory)
                {
                    if (item2.m_shared.m_name == item.m_shared.m_name && item2.m_quality == item.m_quality)
                    {
                        int num = Math.Min(item2.m_shared.m_maxStackSize - item2.m_stack, item.m_stack);
                        item2.m_stack += num;
                        if (item.m_stack == num)
                        {
                            fromInventory.RemoveItem(item);
                            break;
                        }
                        item.m_stack -= num;
                    }
                }
            }
        }
    }
    public static class Inventory_NearbyChests_Cache
    {
        public static List<Container> chests = new List<Container>();

        public static readonly Stopwatch delta = new Stopwatch();
    }


}