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
using Jotunn.Managers;
using System.Runtime.Remoting.Messaging;
using TillValhalla.GameClasses; 
using System.Globalization;
using Object = UnityEngine.Object;
using UnityEngine.PlayerLoop;
using MonoMod.Utils;
using TillValhalla.Configurations;
using TMPro; 

namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Show))]
    public class InventoryGui_Show_Patch
    {
        private const float OneRowSize = 70.5f;
        private const int DefaultPlayerInventoryRows = 4;
        private const int MaxVisiblePlayerInventoryRows = 5;
        private const int MaxPlayerInventoryRows = 20;
        private const int MinPlayerInventoryRows = 4;

        private static float lastValue = 0;
        private static bool baselineCaptured;
        private static float playerBaseHeight;
        private static Vector2 containerBaseOffsetMax;
        private static Vector2 containerBaseOffsetMin;

        public static void Postfix(ref InventoryGui __instance)
        {
            if (!inventoryconfiguration.enabled.Value)
                return;

            int inventoryRows = CalculateInventoryRows(inventoryconfiguration.playerinventoryrows.Value);
            int visibleRows = CalculateVisibleInventoryRows(inventoryRows);

            EnsurePlayerInventoryRows(inventoryRows);
            CaptureLayoutBaseline(__instance);
            ResizePlayerInventory(__instance, visibleRows);
            RepositionContainerInventory(__instance, visibleRows);
            ConfigurePlayerInventoryScrollbar(__instance, inventoryRows > visibleRows);
        }

        private static void EnsurePlayerInventoryRows(int inventoryRows)
        {
            Inventory playerInventory = Player.m_localPlayer?.GetInventory();
            if (playerInventory == null || playerInventory.GetHeight() == inventoryRows)
                return;

            Traverse.Create(playerInventory).Field("m_height").SetValue(inventoryRows);
            Traverse.Create(playerInventory).Method("Changed").GetValue();
        }

        private static void CaptureLayoutBaseline(InventoryGui inventoryGui)
        {
            if (baselineCaptured)
                return;

            RectTransform playerTransform = inventoryGui.m_player;
            RectTransform containerTransform = inventoryGui.m_container;

            playerBaseHeight = playerTransform.rect.height;
            containerBaseOffsetMax = containerTransform.offsetMax;
            containerBaseOffsetMin = containerTransform.offsetMin;
            baselineCaptured = true;
        }

        private static void ResizePlayerInventory(InventoryGui inventoryGui, int inventoryRows)
        {
            RectTransform playerTransform = inventoryGui.m_player;
            float rowDelta = (inventoryRows - DefaultPlayerInventoryRows) * OneRowSize;
            float newHeight = playerBaseHeight + rowDelta;

            playerTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        }

        private static void RepositionContainerInventory(InventoryGui inventoryGui, int inventoryRows)
        {
            RectTransform containerTransform = inventoryGui.m_container;
            float rowDelta = (inventoryRows - DefaultPlayerInventoryRows) * OneRowSize;

            containerTransform.offsetMax = new Vector2(containerBaseOffsetMax.x, containerBaseOffsetMax.y - rowDelta);
            containerTransform.offsetMin = new Vector2(containerBaseOffsetMin.x, containerBaseOffsetMin.y - rowDelta);
        }

        private static void ConfigurePlayerInventoryScrollbar(InventoryGui inventoryGui, bool enableScrolling)
        {
            GameObject playerGrid = inventoryGui.m_playerGrid.gameObject;
            InventoryGrid inventoryGrid = playerGrid.GetComponent<InventoryGrid>();

            if (inventoryGrid.m_scrollbar == null)
            {
                CreateAndConfigureScrollbar(inventoryGui, playerGrid, inventoryGrid);
            }

            ScrollRect scrollRect = playerGrid.GetComponent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.enabled = enableScrolling;
                scrollRect.vertical = enableScrolling;
                if (!enableScrolling)
                {
                    scrollRect.verticalNormalizedPosition = 1f;
                }
            }

            if (inventoryGrid.m_scrollbar != null)
            {
                inventoryGrid.m_scrollbar.gameObject.SetActive(enableScrolling);
                if (!enableScrolling)
                {
                    inventoryGrid.m_scrollbar.value = 1f;
                }
            }
        }

        private static void CreateAndConfigureScrollbar(InventoryGui inventoryGui, GameObject playerGrid, InventoryGrid inventoryGrid)
        {
            GameObject scrollbarGameObject = CreateScrollbarGameObject(playerGrid);
            Scrollbar scrollbar = GetScrollbar(scrollbarGameObject);
            ScrollRect scrollRect = playerGrid.AddComponent<ScrollRect>();

            ConfigureScrollRect(scrollRect, scrollbar, playerGrid, inventoryGui, inventoryGrid);
            CacheScrollbarValue(scrollbar);
        }

        private static GameObject CreateScrollbarGameObject(GameObject playerGrid)
        {
            GameObject containerScrollbar = InventoryGui.instance.m_containerGrid.m_scrollbar.gameObject;
            GameObject scrollbarGameObject = GameObject.Instantiate(containerScrollbar, playerGrid.transform.parent);
            scrollbarGameObject.name = "PlayerScroll";

            return scrollbarGameObject;
        }

        private static void ConfigureScrollRect(ScrollRect scrollRect, Scrollbar scrollbar, GameObject playerGrid, InventoryGui inventoryGui, InventoryGrid inventoryGrid)
        {
            EnableRectMask(playerGrid);
            ConfigureScrollRectProperties(scrollRect, scrollbar, playerGrid, inventoryGui, inventoryGrid);
            ConfigureScrollbarReference(scrollbar, inventoryGrid);
            SetScrollRectBehavior(scrollRect);
        }

        private static void EnableRectMask(GameObject playerGrid)
        {
            RectMask2D rectMask = playerGrid.GetComponent<RectMask2D>();
            rectMask.enabled = true;
        }

        private static void ConfigureScrollRectProperties(ScrollRect scrollRect, Scrollbar scrollbar, GameObject playerGrid, InventoryGui inventoryGui, InventoryGrid inventoryGrid)
        {
            RectTransform playerGridTransform = playerGrid.GetComponent<RectTransform>();

            scrollRect.content = inventoryGrid.m_gridRoot;
            scrollRect.viewport = inventoryGui.m_player.GetComponentInChildren<RectTransform>();
            scrollRect.verticalScrollbar = scrollbar;

            playerGridTransform.offsetMax = new Vector2(800f, playerGridTransform.offsetMax.y);
            playerGridTransform.anchoredPosition = new Vector2(0f, 1f);
        }

        private static void ConfigureScrollbarReference(Scrollbar scrollbar, InventoryGrid inventoryGrid)
        {
            inventoryGrid.m_scrollbar = scrollbar;
        }

        private static void SetScrollRectBehavior(ScrollRect scrollRect)
        {
            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = OneRowSize;
            scrollRect.inertia = false;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        }

        private static void CacheScrollbarValue(Scrollbar scrollbar)
        {
            lastValue = scrollbar.value;
        }

        private static Scrollbar GetScrollbar(GameObject gameObject)
        {
            return gameObject.GetComponent<Scrollbar>();
        }

        private static int CalculateInventoryRows(int configuredRows)
        {
            return (int)Math.Min(MaxPlayerInventoryRows, Math.Max(MinPlayerInventoryRows, configuredRows));
        }

        private static int CalculateVisibleInventoryRows(int inventoryRows)
        {
            return Math.Min(MaxVisiblePlayerInventoryRows, inventoryRows);
        }
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.RepairOneItem))]
    public static class InventoryGui_RepairOneItem_Transpiler
    {
        /// <summary>
        /// Patches out the code that spawns an effect for each item repaired - when we repair multiple items, we only want
        /// one effect, otherwise it looks and sounds bad. The patch for InventoryGui.UpdateRepair will spawn the effect instead.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            if (!PlayerConfiguration.enabled.Value) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            if (PlayerConfiguration.autorepair.Value)
            {
                // Replace any EffectList.Create(...) call with stack cleanup + null return to avoid signature-specific IL issues.
                for (int i = 0; i < il.Count; ++i)
                {
                    if (il[i].operand is MethodInfo calledMethod && calledMethod.DeclaringType == typeof(EffectList) && calledMethod.Name == nameof(EffectList.Create))
                    {
                        int popCount = calledMethod.GetParameters().Length + (calledMethod.IsStatic ? 0 : 1);
                        il[i] = new CodeInstruction(OpCodes.Pop);

                        for (int j = 1; j < popCount; ++j)
                        {
                            il.Insert(i + j, new CodeInstruction(OpCodes.Pop));
                        }

                        il.Insert(i + popCount, new CodeInstruction(OpCodes.Ldnull));
                        i += popCount;
                    }
                }
            }

            return il.AsEnumerable();
        }
    }



	[HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
	public static class InventoryGui_DoCrafting_Transpiler
	{
		private static MethodInfo method_Player_Inventory_RemoveItem = AccessTools.Method(typeof(Inventory), nameof(Inventory.RemoveItem), new Type[] { typeof(string), typeof(int), typeof(int), typeof(bool) });
		private static MethodInfo method_UseItemFromInventoryOrChest = AccessTools.Method(typeof(InventoryGui_DoCrafting_Transpiler), nameof(UseItemFromInventoryOrChest));

		/// <summary>
		/// Patches out the code that's called when crafting.
		/// This changes the call `player.GetInventory().RemoveItem(itemData.m_shared.m_name, amount2, itemData.m_quality);`
		/// to allow crafting recipes with materials comming from containers when they have m_requireOnlyOneIngredient set to True.
		/// </summary>
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
		{
			if (!CraftingStationConfiguration.craftFromChests.Value) return instructions;

			List<CodeInstruction> il = instructions.ToList();

			for (int i = 0; i < il.Count; i++)
			{
				if (il[i].Calls(method_Player_Inventory_RemoveItem))
				{
					il[i].opcode = OpCodes.Call;
					il[i].operand = method_UseItemFromInventoryOrChest;
				}
			}

			return il.AsEnumerable();
		}

		private static void UseItemFromInventoryOrChest(Inventory playerInventory, string itemName, int quantity, int quality, bool worldLevelBased)
		{
			if (playerInventory.CountItems(itemName, quality) >= quantity)
			{
				playerInventory.RemoveItem(itemName, quantity, quality, worldLevelBased);
				return;
			}

			Player player = Player.m_localPlayer;
			if (player == null)
			{
				playerInventory.RemoveItem(itemName, quantity, quality, worldLevelBased);
				return;
			}

			GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
			if (!pos || !CraftingStationConfiguration.craftFromWorkbench.Value) pos = player.gameObject;

			List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(pos, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1, 50), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);

			int toRemove = quantity;
			foreach (Container chest in nearbyChests)
			{
				Inventory chestInventory = chest.GetInventory();
				if (chestInventory.CountItems(itemName, quality) > 0)
				{
					toRemove -= InventoryAssistant.RemoveItemFromChest(chest, itemName, toRemove);
					if (toRemove == 0) return;
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
            TMP_Text component2 = elementRoot.transform.Find("res_name").GetComponent<TMP_Text>();
			TMP_Text component3 = elementRoot.transform.Find("res_amount").GetComponent<TMP_Text>();
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

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRepair))]
    public static class InventoryGui_UpdateRepair_Patch
    {
        /// <summary>
        /// When we're in a state where the InventoryGui is open and we have items available to repair,
        /// and we have an active crafting station, this patch is responsible for repairing all items
        /// that can be repaired and then spawning one instance of the repair effect if at least one item
        /// has been repaired.
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix(InventoryGui __instance)
        {
            if (!PlayerConfiguration.enabled.Value || !PlayerConfiguration.autorepair.Value) return;

            CraftingStation curr_crafting_station = Player.m_localPlayer.GetCurrentCraftingStation();

            if (curr_crafting_station != null)
            {
                int repair_count = 0;

                while (__instance.HaveRepairableItems())
                {
                    __instance.RepairOneItem();
                    ++repair_count;
                }

                if (repair_count > 0)
                {
                    curr_crafting_station.m_repairItemDoneEffects.Create(curr_crafting_station.transform.position, Quaternion.identity, null, 1.0f);
                }
            }
        }
    }

    
}


