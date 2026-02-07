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
                if (name == "Inventory")
                {
                    h = helper.Clamp(inventoryconfiguration.playerinventoryrows.Value, playerInventoryMinRows, playerInventoryMaxRows);
                }
                // Grave inventory - use the stored player inventory size if available
                else if (name == "Grave")
                {
                    if (PlayerInventorySize.LastPlayerInventoryHeight > 0)
                    {
                        h = PlayerInventorySize.LastPlayerInventoryHeight;
                    }
                    else
                    {
                        h = helper.Clamp(inventoryconfiguration.playerinventoryrows.Value, playerInventoryMinRows, playerInventoryMaxRows);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(TombStone), "Awake")]
    public static class TombStone_Awake_Patch
    {
        private const int playerInventoryMaxRows = 20;
        private const int playerInventoryMinRows = 4;

        public static void Postfix(TombStone __instance)
        {
            if (!inventoryconfiguration.enabled.Value || __instance == null)
            {
                return;
            }

            Container container = __instance.GetComponent<Container>();
            if (container == null)
            {
                return;
            }

            // Use the stored player inventory size if available, otherwise fall back to config
            int desiredRows = PlayerInventorySize.LastPlayerInventoryHeight > 0 
                ? PlayerInventorySize.LastPlayerInventoryHeight 
                : helper.Clamp(inventoryconfiguration.playerinventoryrows.Value, playerInventoryMinRows, playerInventoryMaxRows);
            
            // Set container dimensions
            container.m_width = 8;
            container.m_height = desiredRows;

            // If inventory already exists but is wrong size, recreate it
            if (container.m_inventory != null && container.m_inventory.GetHeight() < desiredRows)
            {
                List<ItemDrop.ItemData> existingItems = new List<ItemDrop.ItemData>(container.m_inventory.GetAllItems());
                
                Inventory newInventory = new Inventory("Grave", container.m_inventory.m_bkg, container.m_width, container.m_height);
                
                foreach (ItemDrop.ItemData item in existingItems)
                {
                    newInventory.AddItem(item);
                }
                
                container.m_inventory = newInventory;
                
                // Force update the container's ZDO if networked
                ZNetView nview = container.GetComponent<ZNetView>();
                if (nview != null && nview.IsValid())
                {
                    container.Save();
                }
            }
            
            // Clear the stored size after use
            PlayerInventorySize.LastPlayerInventoryHeight = 0;
        }
    }

    [HarmonyPatch(typeof(Container), "Awake")]
    public static class Container_Awake_Grave_Patch  
    {
        private const int playerInventoryMaxRows = 20;
        private const int playerInventoryMinRows = 4;

        public static void Postfix(Container __instance)
        {
            if (!inventoryconfiguration.enabled.Value || __instance == null)
            {
                return;
            }

            // Check if this is a tombstone container
            TombStone tombStone = __instance.GetComponent<TombStone>();
            if (tombStone != null)
            {
                // Use the stored player inventory size if available, otherwise fall back to config
                int desiredRows = PlayerInventorySize.LastPlayerInventoryHeight > 0 
                    ? PlayerInventorySize.LastPlayerInventoryHeight 
                    : helper.Clamp(inventoryconfiguration.playerinventoryrows.Value, playerInventoryMinRows, playerInventoryMaxRows);
                
                // Override the container dimensions before inventory is created
                __instance.m_width = 8;
                __instance.m_height = desiredRows;
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

    /// <summary>
    /// Stores the player's actual inventory size when they die, so tombstones can be created with the correct size.
    /// This handles server-client sync issues where the player inventory might be larger than the local config.
    /// </summary>
    public static class PlayerInventorySize
    {
        public static int LastPlayerInventoryHeight = 0;
    }
}