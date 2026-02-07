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
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{
    public static class Waterproof
    {
        //No wet debuff
        [HarmonyPatch(typeof(EnvMan), "IsWet")]
        public class EnvMan_Patch
        {

            public static bool Postfix(bool __result)
            {
                if (!PlayerConfiguration.WetFromRain.Value)
                {
                    __result = iswet;
                }
                return __result; 

            }
        }
        [HarmonyPatch(typeof(Player), "UpdateEnvStatusEffects")]
        public static class Player_UpdateEnvStats_Patch
        {
            public static bool Prefix()
            {
                AddingStatFromEnv++;
                return true;
            }

            public static void Postfix(Player __instance)
            {
                AddingStatFromEnv--;
            }
        }

        [HarmonyPatch(typeof(SEMan), "AddStatusEffect", new Type[]
        {
        typeof(int),
        typeof(bool),
        typeof(int),
        typeof(float)
        })]
        public static class SEMan_RemoveWetFromRain_Patch
        {
            public static bool Prefix(SEMan __instance, int nameHash)
            {
                if (!PlayerConfiguration.WetFromRain.Value)
                {
                    if (AddingStatFromEnv > 0 && __instance.m_character.IsPlayer() && nameHash == -1273337594 && iswet)
                    {
                        Player player = (Player)__instance.m_character;

                        return false;
                    }
                }
                return true;
            }
        }

        public static int AddingStatFromEnv;
        public static bool iswet;
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
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public static class SE_Spawned_Patch
    {
        private static void Postfix(Player __instance)
        {
            if (PlayerConfiguration.restedOnStart.Value)
            {


                SEMan seMan = __instance.GetSEMan();
                if (seMan != null)
                {
                    StatusEffect restedEffect = ObjectDB.instance.GetStatusEffect("Rested".GetStableHashCode());
                    if (restedEffect != null)
                    {
                        restedEffect.m_ttl = PlayerConfiguration.restedDurationOnStart.Value;
                        //restedEffect.SetLevel(3, 3); // Set comfort level
                        seMan.AddStatusEffect(restedEffect, resetTime: true);
                        //Jotunn.Logger.LogInfo($"Applied Rested effect to {__instance.GetPlayerName()} for {restedDuration.Value}s with comfort level {comfortLevel.Value}");
                    }
                    else
                    {
                        Jotunn.Logger.LogError("Rested status effect not found!");
                    }
                }
            }
        }

    }

        [HarmonyPatch(typeof(SE_Rested), "GetNearbyComfortPieces")]
    public static class SE_Rested_GetNearbyComfortPieces_Transpiler
    {
		//[HarmonyTranspiler]
		//public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		//{
		//    if (!PlayerConfiguration.enabled.Value || PlayerConfiguration.ComfortRadius.Value == 10f)
		//    {
		//        return instructions;
		//    }
		//    List<CodeInstruction> list = instructions.ToList();
		//    for (int i = 0; i < list.Count; i++)
		//    {
		//        if (list[i].opcode == OpCodes.Ldc_R4)
		//        {
		//            list[i].operand = Mathf.Clamp(PlayerConfiguration.ComfortRadius.Value, 1f, 300f);
		//        }
		//    }
		//    return list.AsEnumerable();
		//}
		private static readonly MethodInfo RangeValueGetter = AccessTools.DeclaredMethod(typeof(SE_Rested_GetNearbyComfortPieces_Transpiler), "getRangeValue", (Type[])null, (Type[])null);

		public static float getRangeValue()
		{
			return 10f;
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			List<CodeInstruction> list = new List<CodeInstruction>(instructions);
			List<CodeInstruction> list2 = new List<CodeInstruction>();
			for (int i = 0; i < list.Count(); i++)
			{
				CodeInstruction val = list[i];
				if (val.opcode == OpCodes.Ldc_R4 && CodeInstructionExtensions.OperandIs(val, (object)10))
				{
					val = new CodeInstruction(OpCodes.Callvirt, (object)RangeValueGetter);
				}
				list2.Add(val);
			}
			return list2;
		}
	}

  //   public static class SE_Rested_Awake_Patch
  //  {
  //      private static void Postfix(SE_Rested __instance)
  //      {
		//	if (PlayerConfiguration.enabled.Value)
  //          {
		//		__instance.m_ = PlayerConfiguration.ComfortRadius.Value;
		//	}
		//}
  //  }
    //[HarmonyPatch]
    //public static class AreaRepair
    //{
    //    private static int m_repair_count;

    //    [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacement))]
    //    public static class Player_UpdatePlacement_Transpiler
    //    {
    //        private static MethodInfo method_Player_Repair = AccessTools.Method(typeof(Player), nameof(Player.Repair));
    //        private static AccessTools.FieldRef<Player, Piece> field_Player_m_hoveringPiece = AccessTools.FieldRefAccess<Player, Piece>(nameof(Player.m_hoveringPiece));
    //        private static MethodInfo method_RepairNearby = AccessTools.Method(typeof(Player_UpdatePlacement_Transpiler), nameof(Player_UpdatePlacement_Transpiler.RepairNearby));

    //        /// <summary>
    //        /// Patches the call to Repair from Player::UpdatePlacement with our own stub which handles repairing multiple pieces rather than just one.
    //        /// </summary>
    //        [HarmonyTranspiler]
    //        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //        {
    //            if (!CraftingStationConfiguration.enabled.Value) return instructions;

    //            List<CodeInstruction> il = instructions.ToList();

    //            if (CraftingStationConfiguration.arearepairenabled.Value)
    //            {
    //                // Replace call to Player::Repair with our own stub.
    //                // Our stub calls the original repair multiple times, one for each nearby piece.
    //                for (int i = 0; i < il.Count; ++i)
    //                {
    //                    if (il[i].Calls(method_Player_Repair))
    //                    {
    //                        il[i].operand = method_RepairNearby;
    //                    }
    //                }
    //            }

    //            return il.AsEnumerable();
    //        }

    //        public static void RepairNearby(Player instance, ItemDrop.ItemData toolItem, Piece _1)
    //        {
    //            Piece selected_piece = instance.GetHoveringPiece();
    //            Vector3 position = selected_piece != null ? selected_piece.transform.position : instance.transform.position;

    //            List<Piece> pieces = new List<Piece>();
    //            Piece.GetAllPiecesInRadius(position, CraftingStationConfiguration.arearepairrange.Value, pieces);

    //            m_repair_count = 0;

    //            Piece original_piece = instance.m_hoveringPiece;

    //            foreach (Piece piece in pieces)
    //            {
    //                bool has_stamina = instance.HaveStamina(toolItem.m_shared.m_attack.m_attackStamina);
    //                bool uses_durability = toolItem.m_shared.m_useDurability;
    //                bool has_durability = toolItem.m_durability > 0.0f;

    //                if (!has_stamina || (uses_durability && !has_durability)) break;

    //                // The repair function takes a piece to repair but then completely ignores it and repairs the hovering piece instead...
    //                // I really don't like this, but Valheim's spaghetti code makes it required.
    //                instance.m_hoveringPiece = piece;
    //                instance.Repair(toolItem, _1);
    //                instance.m_hoveringPiece = original_piece;
    //            }

    //            instance.Message(MessageHud.MessageType.TopLeft, string.Format("{0} pieces repaired", m_repair_count));
    //        }
    //    }

    //    [HarmonyPatch(typeof(Player), nameof(Player.Repair))]
    //    public static class Player_Repair_Transpiler
    //    {
    //        private static MethodInfo method_Character_Message = AccessTools.Method(typeof(Character), nameof(Character.Message));
    //        private static MethodInfo method_MessageNoop = AccessTools.Method(typeof(Player_Repair_Transpiler), nameof(Player_Repair_Transpiler.MessageNoop));

    //        /// <summary>
    //        /// Noops the original message notification when one piece is repaired, and counts them instead - the other transpiler
    //        /// will dispatch one notification for a batch of repairs using this count.
    //        /// </summary>
    //        [HarmonyTranspiler]
    //        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //        {
    //            if (!CraftingStationConfiguration.enabled.Value) return instructions;

    //            List<CodeInstruction> il = instructions.ToList();

    //            if (CraftingStationConfiguration.arearepairenabled.Value)
    //            {
    //                // Replace calls to Character::Message with our own noop stub
    //                // We don't want to spam messages for each piece so we patch the messages out here and dispatch our own messages in the other transpiler.
    //                // First call pushes 1, then subsequent calls 0 - the first call is the branch where the repair succeeded.
    //                int count = 0;
    //                for (int i = 0; i < il.Count; ++i)
    //                {
    //                    if (il[i].Calls(method_Character_Message))
    //                    {
    //                        il[i].operand = method_MessageNoop;
    //                        il.Insert(i++, new CodeInstruction(count++ == 0 ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0, null));
    //                    }
    //                }
    //            }

    //            return il.AsEnumerable();
    //        }

    //        public static void MessageNoop(Character _0, MessageHud.MessageType _1, string _2, int _3, Sprite _4, int repaired)
    //        {
    //            m_repair_count += repaired;
    //        }
    //    }
    //}

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


    [HarmonyPatch(typeof(Player), nameof(Player.HaveRequirementItems), new System.Type[] { typeof(Recipe), typeof(bool),typeof(int) ,typeof(int) })]
    public static class Player_HaveRequirementItems_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), nameof(Inventory.CountItems));
        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirementItems_Transpiler), nameof(Player_HaveRequirementItems_Transpiler.ComputeItemQuantity));

        /// <summary>
        /// Patches out the code that checks if there is enough material to craft a specific object.
        /// The return value of this function is used to set the item as "Craftable" or not in the crafts list.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!CraftingStationConfiguration.craftFromChests.Value) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_Inventory_CountItems))
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_ComputeItemQuantity));
                }
            }

            return il.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
        {
            Stopwatch delta;

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !CraftingStationConfiguration.craftFromWorkbench.Value)
            {
                pos = player.gameObject;
                delta = Inventory_NearbyChests_Cache.delta;
            }
            else
                delta = GameObjectAssistant.GetStopwatch(pos);

            int lookupInterval = helper.Clamp(3, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(pos, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1, 50),  !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
                delta.Restart();
            }
            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.HaveRequirements), new System.Type[] { typeof(Piece), typeof(Player.RequirementMode) })]
    public static class Player_HaveRequirements_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), nameof(Inventory.CountItems));
        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirements_Transpiler), nameof(Player_HaveRequirements_Transpiler.ComputeItemQuantity));

        /// <summary>
        /// Patches out the code that checks if there is enough material to craft a specific object.
        /// The return value of this function determines if the item should be crafted or not.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!CraftingStationConfiguration.craftFromChests.Value) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_Inventory_CountItems))
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_ComputeItemQuantity));
                }
            }

            return il.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
        {
            Stopwatch delta;

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !CraftingStationConfiguration.craftFromWorkbench.Value)
            {
                pos = player.gameObject;
                delta = Inventory_NearbyChests_Cache.delta;
            }
            else
                delta = GameObjectAssistant.GetStopwatch(pos);

            int lookupInterval = helper.Clamp(3, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(pos, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1, 50), !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
                delta.Restart();
            }

            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeResources), new Type[] { typeof(Piece.Requirement[]), typeof(int), typeof(int), typeof(int) })]
    public static class Player_ConsumeResources_Transpiler
    {
        private static MethodInfo method_Inventory_RemoveItem = AccessTools.Method(typeof(Inventory), nameof(Inventory.RemoveItem), new Type[] { typeof(string), typeof(int), typeof(int), typeof(bool) });
        private static MethodInfo method_RemoveItemsFromInventoryAndNearbyChests = AccessTools.Method(typeof(Player_ConsumeResources_Transpiler), nameof(Player_ConsumeResources_Transpiler.RemoveItemsFromInventoryAndNearbyChests));

        /// <summary>
        /// Patches out the code that consumes the material required to craft something.
        /// We first remove the amount we can from the player inventory before moving on to the nearby chests.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!CraftingStationConfiguration.craftFromChests.Value) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            int thisIdx = -1;
            int callIdx = -1;

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].opcode == OpCodes.Ldarg_0)
                {
                    thisIdx = i;
                }
                else if (il[i].Calls(method_Inventory_RemoveItem))
                {
                    callIdx = i;
                    break;
                }
            }

            if (thisIdx == -1 || callIdx == -1)
            {
                ZLog.LogError("Failed to apply Player_ConsumeResources_Transpiler");
                return instructions;
            }
            il.RemoveRange(thisIdx + 1, callIdx - thisIdx);

            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Ldloc_2));
            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Ldloc_3));
            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Ldarg_3));
            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Call, method_RemoveItemsFromInventoryAndNearbyChests));

            return il.AsEnumerable();
        }

        private static void RemoveItemsFromInventoryAndNearbyChests(Player player, Piece.Requirement item, int amount, int itemQuality)
        {
            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !CraftingStationConfiguration.craftFromWorkbench.Value) pos = player.gameObject;

            int inventoryAmount = player.m_inventory.CountItems(item.m_resItem.m_itemData.m_shared.m_name);
            player.m_inventory.RemoveItem(item.m_resItem.m_itemData.m_shared.m_name, amount, itemQuality);
            amount -= inventoryAmount;
            if (amount <= 0) return;

            InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(pos, helper.Clamp(CraftingStationConfiguration.craftFromChestRange.Value, 1, 50), item.m_resItem.m_itemData, amount, !CraftingStationConfiguration.ignorePrivateAreaCheck.Value);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.GetFirstRequiredItem))]
    public static class Player_GetFirstRequiredItem_Transpiler
    {
        /// <summary>
        /// Patches out the function Player::GetFirstRequiredItem
        /// As the original code is calling Inventory::CountItems using `this` instead of using the inventory parameter
        /// we can't use this function to check get the first required item from a Container (chest) inventory.
        /// Instead of calling `this.m_inventory.CountItems` we're now calling `inventory.CountItems`.
        ///
        /// As the original function passes `this.GetInventory()` where `this` is the Player instance as the inventory parameter,
        /// there is no change to the way the function would usually work.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Ldarg_0)
                {
                    il[i].opcode = OpCodes.Ldarg_1;
                    il.RemoveAt(i + 1);

                    return il.AsEnumerable();
                }
            }
            return instructions;
        }
    }

    [HarmonyPatch(typeof(Player), "OnDeath")]
    public static class Player_OnDeath_Patch
    {
        /// <summary>
        /// Captures the player's actual inventory size when they die.
        /// This ensures the tombstone is created with the correct size, even if the server's config differs from the client's.
        /// </summary>
        public static void Prefix(Player __instance)
        {
            if (!inventoryconfiguration.enabled.Value || __instance == null)
            {
                return;
            }

            Inventory playerInventory = __instance.GetInventory();
            if (playerInventory != null)
            {
                // Store the actual runtime inventory height
                PlayerInventorySize.LastPlayerInventoryHeight = playerInventory.GetHeight();
                
                if (SmelterConfiguration.enableDebugLogging != null && SmelterConfiguration.enableDebugLogging.Value)
                {
                    ZLog.Log($"Player {__instance.GetPlayerName()} died with inventory height: {PlayerInventorySize.LastPlayerInventoryHeight}");
                }
            }
        }
    }
}

