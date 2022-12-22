using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TillValhalla.GameClasses;
using System.Linq;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Diagnostics;

namespace TillValhalla.GameClasses
{
    public static class SmelterDefinitions
    {
        public static readonly string KilnName = "$piece_charcoalkiln";

        public static readonly string SmelterName = "$piece_smelter";

        public static readonly string FurnaceName = "$piece_blastfurnace";

        public static readonly string WindmillName = "$piece_windmill";

        public static readonly string SpinningWheelName = "$piece_spinningwheel";
    }
    public static class WoodDefinitions
    {
        public static readonly string FineWoodName = "$item_finewood";

        public static readonly string RoundLogName = "$item_roundlog";
    }

    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class Smelter_Awake_Patch
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (__instance.m_name.Equals(SmelterDefinitions.KilnName) && SmelterConfiguration.kilnIsEnabled.Value)
            {
                __instance.m_maxOre = SmelterConfiguration.kilnMaxWood.Value;
                __instance.m_secPerProduct = SmelterConfiguration.kilnSpeed.Value;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SmelterName) && SmelterConfiguration.smelterIsEnabled.Value)
            {
                __instance.m_maxOre = SmelterConfiguration.smelterMaxOre.Value;
                __instance.m_maxFuel = SmelterConfiguration.smelterMaxCoal.Value;
                __instance.m_secPerProduct = SmelterConfiguration.smelterSpeed.Value;
                __instance.m_fuelPerProduct = SmelterConfiguration.smelterCoalUsedPerProduct.Value;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName) && SmelterConfiguration.furnaceIsEnabled.Value)
            {
                __instance.m_maxOre = SmelterConfiguration.furnaceMaxOre.Value;
                __instance.m_maxFuel = SmelterConfiguration.furnaceMaxCoal.Value;
                __instance.m_secPerProduct = SmelterConfiguration.furnaceSpeed.Value;
                __instance.m_fuelPerProduct = SmelterConfiguration.furnaceCoalUsedPerProduct.Value;
                //if (Configuration.Current.Furnace.allowAllOres)
                //{
                //    __instance.m_conversion.AddRange(FurnaceDefinitions.AdditionalConversions);
                //}
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.WindmillName) && SmelterConfiguration.windmillIsEnabled.Value)
            {
                __instance.m_maxOre = SmelterConfiguration.windmillMaxBarley.Value;
                __instance.m_secPerProduct = SmelterConfiguration.windmillProductionSpeed.Value;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName) && SmelterConfiguration.spinningWheelIsEnabled.Value)
            {
                __instance.m_maxOre = SmelterConfiguration.spinningWheelMaxFlax.Value;
                __instance.m_secPerProduct = SmelterConfiguration.spinningWheelProductionSpeed.Value;
            }
        }
    }

    [HarmonyPatch(typeof(Smelter), "FindCookableItem")]
    public static class Smelter_FindCookableItem_Transpiler
    {
        private static MethodInfo method_PreventUsingSpecificWood = AccessTools.Method(typeof(Smelter_FindCookableItem_Transpiler), "PreventUsingSpecificWood", (Type[])null, (Type[])null);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //IL_005c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0066: Expected O, but got Unknown
            //IL_0075: Unknown result type (might be due to invalid IL or missing references)
            //IL_007f: Expected O, but got Unknown
            //IL_0092: Unknown result type (might be due to invalid IL or missing references)
            //IL_009c: Expected O, but got Unknown
            //IL_00e0: Unknown result type (might be due to invalid IL or missing references)
            //IL_00ea: Expected O, but got Unknown
            if (!SmelterConfiguration.kilnIsEnabled.Value)
            {
                return instructions;
            }
            int num = -1;
            List<CodeInstruction> list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Stloc_1)
                {
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldloc_1, (object)null));
                    list.Insert(++i, new CodeInstruction(OpCodes.Call, (object)method_PreventUsingSpecificWood));
                    num = i;
                }
                else if (num != -1 && list[i].opcode == OpCodes.Brfalse)
                {
                    list.Insert(++num, new CodeInstruction(OpCodes.Brtrue, list[i].operand));
                    return list.AsEnumerable();
                }
            }
            ZLog.LogError("Failed to apply Smelter_FindCookableItem_Transpiler");
            return instructions;
        }

        private static bool PreventUsingSpecificWood(Smelter smelter, Smelter.ItemConversion itemConversion)
        {
            if (smelter.m_name.Equals(SmelterDefinitions.KilnName) && ((SmelterConfiguration.dontProcessFineWood.Value && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.FineWoodName)) || (SmelterConfiguration.dontProcessRoundLog.Value && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.RoundLogName))))
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Smelter), "Spawn")]
    public static class Smelter_Spawn_Patch
    {
        private static bool Prefix(string ore, int stack, ref Smelter __instance)
        {
            Smelter smelter = __instance;
            if (!smelter.m_nview.IsOwner())
            {
                return true;
            }
            if (__instance.m_name.Equals(SmelterDefinitions.KilnName) && SmelterConfiguration.kilnIsEnabled.Value && SmelterConfiguration.kilnAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.kilnAutoRange.Value, 1f, 50f), false); //Configuration.Current.Kiln.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.SmelterName) && SmelterConfiguration.smelterIsEnabled.Value && SmelterConfiguration.smelterAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.smelterAutoRange.Value, 1f, 50f), false); //Configuration.Current.Smelter.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName) && SmelterConfiguration.furnaceIsEnabled.Value && SmelterConfiguration.furnaceAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.furnaceAutoRange.Value, 1f, 50f), false); // Configuration.Current.Furnace.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.WindmillName) && SmelterConfiguration.windmillIsEnabled.Value && SmelterConfiguration.windmillAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.windmillAutoRange.Value, 1f, 50f), false); // Configuration.Current.Windmill.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName) && SmelterConfiguration.spinningWheelIsEnabled.Value && SmelterConfiguration.spinningWheelAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.spinningWheelAutoRange.Value, 1f, 50f), false); // Configuration.Current.Windmill.ignorePrivateAreaCheck);
            }
            return true;
            bool spawn(float autoDepositRange, bool ignorePrivateAreaCheck)
            {
                List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(smelter.gameObject, autoDepositRange, !ignorePrivateAreaCheck);
                if (nearbyChests.Count == 0)
                {
                    return true;
                }
                if (autoDepositRange > 50f)
                {
                    autoDepositRange = 50f;
                }
                else if (autoDepositRange < 1f)
                {
                    autoDepositRange = 1f;
                }
                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(smelter.GetItemConversion(ore).m_to.gameObject.name);
                ZNetView.m_forceDisableInit = true;
                GameObject gameObject = UnityEngine.Object.Instantiate(itemPrefab);
                ZNetView.m_forceDisableInit = false;
                ItemDrop comp = gameObject.GetComponent<ItemDrop>();
                comp.m_itemData.m_stack = stack;
                bool result = spawnNearbyChest(mustHaveItem: true);
                UnityEngine.Object.Destroy(gameObject);
                return result;
                bool spawnNearbyChest(bool mustHaveItem)
                {
                    foreach (Container item in nearbyChests)
                    {
                        Inventory inventory = item.GetInventory();
                        if ((!mustHaveItem || inventory.HaveItem(comp.m_itemData.m_shared.m_name)) && inventory.AddItem(comp.m_itemData))
                        {
                            smelter.m_produceEffects.Create(smelter.transform.position, smelter.transform.rotation);
                            InventoryAssistant.ConveyContainerToNetwork(item);
                            return false;
                        }
                    }
                    if (mustHaveItem)
                    {
                        return spawnNearbyChest(mustHaveItem: false);
                    }
                    return true;
                }
            }
        }
    }

    //[HarmonyPatch(typeof(Smelter), "UpdateSmelter")]
    //public static class Smelter_UpdaterSmelter_Transpiler
    //{
    //    private static MethodInfo method_Windmill_GetPowerOutput = AccessTools.Method(typeof(Windmill), "GetPowerOutput", (Type[])null, (Type[])null);

    //    private static MethodInfo method_GetPowerOutput = AccessTools.Method(typeof(Smelter_UpdaterSmelter_Transpiler), "GetPowerOutput", (Type[])null, (Type[])null);

    //    [HarmonyTranspiler]
    //    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //    {
    //        if (!SmelterConfiguration.windmillIsEnabled.Value )//|| !Configuration.Current.Windmill.ignoreWindIntensity)
    //        {
    //            return instructions;
    //        }
    //        List<CodeInstruction> list = instructions.ToList();
    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (CodeInstructionExtensions.Calls(list[i], method_Windmill_GetPowerOutput))
    //            {
    //                list[i].operand = method_GetPowerOutput;
    //                return list.AsEnumerable();
    //            }
    //        }
    //        return instructions;
    //    }

    //    private static float GetPowerOutput(Windmill __instance)
    //    {
    //        return 1f;
    //    }
    //}


    [HarmonyPatch(typeof(Smelter), "UpdateSmelter")]
    public static class Smelter_UpdateSmelter_Patch
    {
        private static void Prefix(Smelter __instance)
        {
            if (__instance == null || !Player.m_localPlayer || __instance.m_nview == null || !__instance.m_nview.IsOwner())
            {
                return;
            }
            Stopwatch stopwatch = GameObjectAssistant.GetStopwatch(__instance.gameObject);
            if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds < 1000)
            {
                return;
            }
            stopwatch.Restart();
            float value = 0f;
            bool flag = false;
            bool flag2 = false;
            if (__instance.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (!SmelterConfiguration.kilnIsEnabled.Value || !SmelterConfiguration.kilnAutoFuel.Value)
                {
                    return;
                }
                flag2 = true;
                value = SmelterConfiguration.kilnAutoRange.Value;
                flag = false; //Configuration.Current.Kiln.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SmelterName))
            {
                if (!SmelterConfiguration.smelterIsEnabled.Value || !SmelterConfiguration.smelterAutoFuel.Value)
                {
                    return;
                }
                value = SmelterConfiguration.smelterAutoRange.Value;
                flag = false; //Configuration.Current.Smelter.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName))
            {
                if (!SmelterConfiguration.furnaceIsEnabled.Value || !SmelterConfiguration.furnaceAutoFuel.Value)
                {
                    return;
                }
                value = SmelterConfiguration.furnaceAutoRange.Value;
                flag = false; // Configuration.Current.Furnace.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.WindmillName))
            {
                if (!SmelterConfiguration.windmillIsEnabled.Value || !SmelterConfiguration.windmillAutoFuel.Value)
                {
                    return;
                }
                value = SmelterConfiguration.windmillAutoRange.Value;
                flag = false; //Configuration.Current.Windmill.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName))
            {
                if (!SmelterConfiguration.spinningWheelIsEnabled.Value|| !SmelterConfiguration.spinningWheelAutoFuel.Value)
                {
                    return;
                }
                value = SmelterConfiguration.spinningWheelAutoRange.Value;
                flag = false; // Configuration.Current.SpinningWheel.ignorePrivateAreaCheck;
            }
            value = helper.Clamp(value, 1f, 50f);
            int num = __instance.m_maxOre - __instance.GetQueueSize();
            int num2 = __instance.m_maxFuel - (int)Math.Ceiling(__instance.GetFuel());
            if ((bool)__instance.m_fuelItem && num2 > 0)
            {
                ItemDrop.ItemData itemData = __instance.m_fuelItem.m_itemData;
                int num3 = InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(__instance.gameObject, value, itemData, num2, !flag);
                for (int i = 0; i < num3; i++)
                {
                    __instance.m_nview.InvokeRPC("AddFuel");
                }
                if (num3 > 0)
                {
                    ZLog.Log("Added " + num3 + " fuel(" + itemData.m_shared.m_name + ") in " + __instance.m_name);
                }
            }
            if (num <= 0)
            {
                return;
            }
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(__instance.gameObject, value);
            foreach (Container item in nearbyChests)
            {
                foreach (Smelter.ItemConversion item2 in __instance.m_conversion)
                {
                    if (flag2)
                    {
                        if ((SmelterConfiguration.dontProcessFineWood.Value && item2.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.FineWoodName)) || (SmelterConfiguration.dontProcessRoundLog.Value && item2.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.RoundLogName)))
                        {
                            continue;
                        }
                        int num4 = ((SmelterConfiguration.kilnStopAutoFuelThreshold.Value >= 0) ? SmelterConfiguration.kilnStopAutoFuelThreshold.Value : 0);
                        if (num4 > 0 && InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(nearbyChests), item2.m_to.m_itemData) >= num4)
                        {
                            return;
                        }
                    }
                    ItemDrop.ItemData itemData2 = item2.m_from.m_itemData;
                    int num5 = InventoryAssistant.RemoveItemFromChest(item, itemData2, num);
                    if (num5 > 0)
                    {
                        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(item2.m_from.gameObject.name);
                        for (int j = 0; j < num5; j++)
                        {
                            __instance.m_nview.InvokeRPC("AddOre", itemPrefab.name);
                        }
                        num -= num5;
                        if (num5 > 0)
                        {
                            ZLog.Log("Added " + num5 + " ores(" + itemData2.m_shared.m_name + ") in " + __instance.m_name);
                        }
                        if (num == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

}

