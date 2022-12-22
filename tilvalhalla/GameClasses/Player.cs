using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.GameClasses;
using TillValhalla;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;


namespace TillValhalla.GameClasses
{

    //No wet debuff
    [HarmonyPatch(typeof(EnvMan), "IsWet")]
    internal class EnvMan_Patch
    {
        public static bool Postfix(bool __result)
        {


            __result = false;
            return __result;

        }
    }

    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake_Patch
    {
        private static void Postfix(Player __instance)
        {
            if (PlayerConfiguration.enabled.Value)
            {
                __instance.m_maxCarryWeight = PlayerConfiguration.basemaximumweight.Value;
                __instance.m_baseHP = PlayerConfiguration.basehp.Value;
                __instance.m_baseStamina = PlayerConfiguration.basestamina.Value;
            }
        }
    }



    [HarmonyPatch(typeof(SE_Stats), nameof(SE_Stats.Setup))]
    public static class SE_Stats_Setup_Patch
    {
        private static void Postfix(ref SE_Stats __instance)
        {
            if (PlayerConfiguration.enabled.Value)
                if (__instance.m_addMaxCarryWeight != null && __instance.m_addMaxCarryWeight > 0)
                    __instance.m_addMaxCarryWeight = (__instance.m_addMaxCarryWeight - 150) + PlayerConfiguration.baseMegingjordBuff.Value;
        }
    }

    [HarmonyPatch(typeof(Player), "HaveRequirementItems", new Type[]
{
    typeof(Recipe),
    typeof(bool),
    typeof(int)
})]
    public static class Player_HaveRequirementItems_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), "CountItems", (Type[])null, (Type[])null);

        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirementItems_Transpiler), "ComputeItemQuantity", (Type[])null, (Type[])null);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //IL_004e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0058: Expected O, but got Unknown
            //IL_0065: Unknown result type (might be due to invalid IL or missing references)
            //IL_006f: Expected O, but got Unknown
            //IL_0080: Unknown result type (might be due to invalid IL or missing references)
            //IL_008a: Expected O, but got Unknown
            if (!CraftingStationConfiguration.craftFromChests.Value)
            {
                return instructions;
            }
            List<CodeInstruction> list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (CodeInstructionExtensions.Calls(list[i], method_Inventory_CountItems))
                {
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Call, (object)method_ComputeItemQuantity));
                }
            }
            return list.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
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
            int num = helper.Clamp(3, 1, 10) * 1000;
            if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds > num)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(gameObject, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1f, 50f), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
                stopwatch.Restart();
            }
            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }

    [HarmonyPatch(typeof(Player), "HaveRequirements", new Type[]
{
    typeof(Piece),
    typeof(Player.RequirementMode)
})]
    public static class Player_HaveRequirements_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), "CountItems", (Type[])null, (Type[])null);

        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirements_Transpiler), "ComputeItemQuantity", (Type[])null, (Type[])null);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //IL_004e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0058: Expected O, but got Unknown
            //IL_0065: Unknown result type (might be due to invalid IL or missing references)
            //IL_006f: Expected O, but got Unknown
            //IL_0080: Unknown result type (might be due to invalid IL or missing references)
            //IL_008a: Expected O, but got Unknown
            if (!CraftingStationConfiguration.craftFromChests.Value)
            {
                return instructions;
            }
            List<CodeInstruction> list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (CodeInstructionExtensions.Calls(list[i], method_Inventory_CountItems))
                {
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Call, (object)method_ComputeItemQuantity));
                }
            }
            return list.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
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
            int num = helper.Clamp(3, 1, 10) * 1000;
            if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds > num)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(gameObject, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1f, 50f), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
                stopwatch.Restart();
            }
            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }



}

