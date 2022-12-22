using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using TillValhalla.Configurations.Sections;
using System.Runtime.Remoting.Messaging;
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Show))]
    public class InventoryGui_Show_Patch
    {
        private const float oneRowSize = 70.5f;
        private const float containerOriginalY = -90.0f;
        private const float containerHeight = -340.0f;
        private static float lastValue = 0;
        
        public static void Postfix(ref InventoryGui __instance)
        {
            if (inventoryconfiguration.enabled.Value)
            {
                RectTransform container = __instance.m_container;
                RectTransform player = __instance.m_player;
                GameObject playerGrid = InventoryGui.instance.m_playerGrid.gameObject;

                // Player inventory background size, only enlarge it up to 6x8 rows, after that use the scroll bar
                int playerInventoryBackgroundSize = Math.Min(6, Math.Max(4, inventoryconfiguration.playerinventoryrows.Value));
                float containerNewY = containerOriginalY - oneRowSize * playerInventoryBackgroundSize;
                // Resize player inventory
                player.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, playerInventoryBackgroundSize * oneRowSize);
                // Move chest inventory based on new player invetory size
                container.offsetMax = new Vector2(610, containerNewY);
                container.offsetMin = new Vector2(40, containerNewY + containerHeight);

                // Add player inventory scroll bar if it does not exist
                if (!playerGrid.GetComponent<InventoryGrid>().m_scrollbar)
                {
                    GameObject playerGridScroll = GameObject.Instantiate(InventoryGui.instance.m_containerGrid.m_scrollbar.gameObject, playerGrid.transform.parent);
                    playerGridScroll.name = "PlayerScroll";
                    playerGrid.GetComponent<RectMask2D>().enabled = true;
                    ScrollRect playerScrollRect = playerGrid.AddComponent<ScrollRect>();
                    playerGrid.GetComponent<RectTransform>().offsetMax = new Vector2(800f, playerGrid.GetComponent<RectTransform>().offsetMax.y);
                    playerGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1f);
                    playerScrollRect.content = playerGrid.GetComponent<InventoryGrid>().m_gridRoot;
                    playerScrollRect.viewport = __instance.m_player.GetComponentInChildren<RectTransform>();
                    playerScrollRect.verticalScrollbar = playerGridScroll.GetComponent<Scrollbar>();
                    playerGrid.GetComponent<InventoryGrid>().m_scrollbar = playerGridScroll.GetComponent<Scrollbar>();

                    playerScrollRect.horizontal = false;
                    playerScrollRect.movementType = ScrollRect.MovementType.Clamped;
                    playerScrollRect.scrollSensitivity = oneRowSize;
                    playerScrollRect.inertia = false;
                    playerScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
                    Scrollbar playerScrollbar = playerGridScroll.GetComponent<Scrollbar>();
                    lastValue = playerScrollbar.value;
                }
            }
        }
    }
    [HarmonyPatch(typeof(InventoryGui), "DoCrafting")]
    public static class InventoryGui_DoCrafting_Transpiler
    {
        private static MethodInfo method_Player_Inventory_RemoveItem = AccessTools.Method(typeof(Inventory), "RemoveItem", new Type[3]
        {
        typeof(string),
        typeof(int),
        typeof(int)
        }, (Type[])null);

        private static MethodInfo method_Player_GetFirstRequiredItem = AccessTools.Method(typeof(Player), "GetFirstRequiredItem", (Type[])null, (Type[])null);

        private static MethodInfo method_UseItemFromIventoryOrChest = AccessTools.Method(typeof(InventoryGui_DoCrafting_Transpiler), "UseItemFromIventoryOrChest", (Type[])null, (Type[])null);

        private static MethodInfo method_GetFirstRequiredItemFromInventoryOrChest = AccessTools.Method(typeof(InventoryGui_DoCrafting_Transpiler), "GetFirstRequiredItemFromInventoryOrChest", (Type[])null, (Type[])null);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            //IL_004e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0058: Expected O, but got Unknown
            //IL_00a6: Unknown result type (might be due to invalid IL or missing references)
            //IL_00b0: Expected O, but got Unknown
            if (!CraftingStationConfiguration.craftFromChests.Value)
            {
                return instructions;
            }
            List<CodeInstruction> list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (CodeInstructionExtensions.Calls(list[i], method_Player_GetFirstRequiredItem))
                {
                    list[i] = new CodeInstruction(OpCodes.Call, (object)method_GetFirstRequiredItemFromInventoryOrChest);
                    list.RemoveRange(i - 6, 2);
                    break;
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                if (CodeInstructionExtensions.Calls(list[j], method_Player_Inventory_RemoveItem))
                {
                    list[j] = new CodeInstruction(OpCodes.Call, (object)method_UseItemFromIventoryOrChest);
                    list.RemoveAt(j - 7);
                    return list.AsEnumerable();
                }
            }
            return instructions;
        }

        private static ItemDrop.ItemData GetFirstRequiredItemFromInventoryOrChest(Player player, Recipe recipe, int quality, out int quantity)
        {
            ItemDrop.ItemData firstRequiredItem = player.GetFirstRequiredItem(player.GetInventory(), recipe, quality, out quantity);
            if (firstRequiredItem != null)
            {
                return firstRequiredItem;
            }
            GameObject gameObject = player.GetCurrentCraftingStation()?.gameObject;
            if (!gameObject || !CraftingStationConfiguration.craftFromWorkbench.Value)
            {
                gameObject = player.gameObject;
            }
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(gameObject, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1f, 50f), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
            foreach (Container item in nearbyChests)
            {
                firstRequiredItem = player.GetFirstRequiredItem(item.GetInventory(), recipe, quality, out quantity);
                if (firstRequiredItem != null)
                {
                    return firstRequiredItem;
                }
            }
            return null;
        }

        private static void UseItemFromIventoryOrChest(Player player, string itemName, int quantity, int quality)
        {
            Inventory inventory = player.GetInventory();
            if (inventory.CountItems(itemName, quality) >= quantity)
            {
                inventory.RemoveItem(itemName, quantity, quality);
                return;
            }
            GameObject gameObject = player.GetCurrentCraftingStation()?.gameObject;
            if (!gameObject || !CraftingStationConfiguration.craftFromWorkbench.Value)
            {
                gameObject = player.gameObject;
            }
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(gameObject, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1f, 50f), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
            int num = quantity;
            foreach (Container item in nearbyChests)
            {
                Inventory inventory2 = item.GetInventory();
                if (inventory2.CountItems(itemName, quality) > 0)
                {
                    num -= InventoryAssistant.RemoveItemFromChest(item, itemName, num);
                    if (num == 0)
                    {
                        break;
                    }
                }
            }
        }
    }
    [HarmonyPatch(typeof(InventoryGui), "SetupRequirement")]
    public static class InventoryGui_SetupRequirement_Patch
    {
        private static bool Prefix(Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality, ref bool __result)
        {
            if ((!CraftingStationConfiguration.craftFromChests.Value))
            {
                return true;
            }
            Image component = elementRoot.transform.Find("res_icon").GetComponent<Image>();
            Text component2 = elementRoot.transform.Find("res_name").GetComponent<Text>();
            Text component3 = elementRoot.transform.Find("res_amount").GetComponent<Text>();
            UITooltip component4 = elementRoot.GetComponent<UITooltip>();
            if (req.m_resItem != null)
            {
                component.gameObject.SetActive(value: true);
                component2.gameObject.SetActive(value: true);
                component3.gameObject.SetActive(value: true);
                component.sprite = req.m_resItem.m_itemData.GetIcon();
                component.color = Color.white;
                component4.m_text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
                component2.text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
                int num = player.GetInventory().CountItems(req.m_resItem.m_itemData.m_shared.m_name);
                int amount = req.GetAmount(quality);
                if (amount <= 0)
                {
                    InventoryGui.HideRequirement(elementRoot);
                    __result = false;
                    return false;
                }
                if (CraftingStationConfiguration.craftFromChests.Value)
                {
                    GameObject gameObject = player.GetCurrentCraftingStation()?.gameObject;
                    Stopwatch stopwatch;
                    if (!gameObject || !CraftingStationConfiguration.craftFromWorkbench.Value)
                    {
                        gameObject = player.gameObject;
                        stopwatch = Inventory_NearbyChests_Cache.delta;
                    }
                    else
                    {
                        stopwatch = GameObjectAssistant.GetStopwatch(gameObject);
                    }
                    int num2 = helper.Clamp(3, 1, 10) * 1000;
                    if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds > num2)
                    {
                        Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(gameObject, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1f, 50f));
                        stopwatch.Restart();
                    }
                    num += InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), req.m_resItem.m_itemData);
                }
                component3.text = num + "/" + amount;
                if (num < amount)
                {
                    component3.color = ((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red : Color.white);
                }
                else
                {
                    component3.color = Color.white;
                }
                component3.fontSize = 14;
                if (component3.text.Length > 5)
                {
                    component3.fontSize -= component3.text.Length - 5;
                }
            }
            __result = true;
            return false;
        }
    }

}