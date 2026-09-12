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
using TillValhalla; 

namespace TillValhalla.GameClasses
{
    public static class SmelterDefinitions
    {
        public static readonly string KilnName = "$piece_charcoalkiln";

        public static readonly string SmelterName = "$piece_smelter";

        public static readonly string FurnaceName = "$piece_blastfurnace";

        public static readonly string WindmillName = "$piece_windmill";

        public static readonly string SpinningWheelName = "$piece_spinningwheel";

        public static readonly string EitrRefineryName = "$piece_eitrrefinery";

    }
    

    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class Smelter_Awake_Patch
    {
        private static Dictionary<int, SmelterType> smelterTypeCache = new Dictionary<int, SmelterType>();

        public enum SmelterType
        {
            Unknown,
            Kiln,
            Smelter,
            Furnace,
            Windmill,
            SpinningWheel,
            EitrRefinery
        }

        private static void Prefix(ref Smelter __instance)
        {
            // Determine and cache smelter type to avoid repeated string comparisons
            SmelterType type = DetermineSmelterType(__instance);
            int instanceId = __instance.GetInstanceID();
            smelterTypeCache[instanceId] = type;

            switch (type)
            {
                case SmelterType.Kiln:
                    if (SmelterConfiguration.kilnIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.kilnMaxWood.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.kilnSpeed.Value;
                    }
                    break;

                case SmelterType.Smelter:
                    if (SmelterConfiguration.smelterIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.smelterMaxOre.Value;
                        __instance.m_maxFuel = SmelterConfiguration.smelterMaxCoal.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.smelterSpeed.Value;
                        __instance.m_fuelPerProduct = SmelterConfiguration.smelterCoalUsedPerProduct.Value;
                    }
                    break;

                case SmelterType.Furnace:
                    if (SmelterConfiguration.furnaceIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.furnaceMaxOre.Value;
                        __instance.m_maxFuel = SmelterConfiguration.furnaceMaxCoal.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.furnaceSpeed.Value;
                        __instance.m_fuelPerProduct = SmelterConfiguration.furnaceCoalUsedPerProduct.Value;
                    }
                    break;

                case SmelterType.Windmill:
                    if (SmelterConfiguration.windmillIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.windmillMaxBarley.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.windmillProductionSpeed.Value;
                    }
                    break;

                case SmelterType.SpinningWheel:
                    if (SmelterConfiguration.spinningWheelIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.spinningWheelMaxFlax.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.spinningWheelProductionSpeed.Value;
                    }
                    break;

                case SmelterType.EitrRefinery:
                    if (SmelterConfiguration.eitrRefineryIsEnabled.Value)
                    {
                        __instance.m_maxOre = SmelterConfiguration.eitrRefineryMaxOre.Value;
                        __instance.m_maxFuel = SmelterConfiguration.eitrRefineryMaxCoal.Value;
                        __instance.m_secPerProduct = SmelterConfiguration.eitrRefinerySpeed.Value;
                        __instance.m_fuelPerProduct = SmelterConfiguration.eitrRefineryCoalUsedPerProduct.Value;
                    }
                    break;
            }
        }

        private static SmelterType DetermineSmelterType(Smelter smelter)
        {
            string name = smelter.m_name;
            
            if (name == SmelterDefinitions.KilnName) return SmelterType.Kiln;
            if (name == SmelterDefinitions.SmelterName) return SmelterType.Smelter;
            if (name == SmelterDefinitions.FurnaceName) return SmelterType.Furnace;
            if (name == SmelterDefinitions.WindmillName) return SmelterType.Windmill;
            if (name == SmelterDefinitions.SpinningWheelName) return SmelterType.SpinningWheel;
            if (name == SmelterDefinitions.EitrRefineryName) return SmelterType.EitrRefinery;
            
            return SmelterType.Unknown;
        }

        public static SmelterType GetCachedSmelterType(Smelter smelter)
        {
            int instanceId = smelter.GetInstanceID();
            if (smelterTypeCache.TryGetValue(instanceId, out SmelterType type))
            {
                return type;
            }
            return SmelterType.Unknown;
        }
    }

    [HarmonyPatch(typeof(Smelter), "FindCookableItem")]
    public static class Smelter_FindCookableItem_Transpiler
    {
        private static MethodInfo method_PreventUsingSpecificWood = AccessTools.Method(typeof(Smelter_FindCookableItem_Transpiler), "PreventUsingSpecificWood", (Type[])null, (Type[])null);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
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

            MethodBody methodBody = original?.GetMethodBody();
            LocalVariableInfo conversionLocal = methodBody?.LocalVariables?.FirstOrDefault(local => local.LocalType == typeof(Smelter.ItemConversion));
            if (conversionLocal == null)
            {
                ZLog.LogWarning("Smelter_FindCookableItem_Transpiler: could not find ItemConversion local; skipping patch");
                return instructions;
            }

            int insertAfterIndex = -1;
            List<CodeInstruction> list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (IsStloc(list[i], conversionLocal.LocalIndex))
                {
                    list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    list.Insert(++i, LoadLocal(conversionLocal.LocalIndex));
                    list.Insert(++i, new CodeInstruction(OpCodes.Call, method_PreventUsingSpecificWood));
                    insertAfterIndex = i;
                }
                else if (insertAfterIndex != -1 && list[i].opcode == OpCodes.Brfalse)
                {
                    list.Insert(++insertAfterIndex, new CodeInstruction(OpCodes.Brtrue, list[i].operand));
                    return list.AsEnumerable();
                }
            }
            ZLog.LogError("Failed to apply Smelter_FindCookableItem_Transpiler");
            return instructions;
        }

        private static bool IsStloc(CodeInstruction instruction, int localIndex)
        {
            switch (localIndex)
            {
                case 0: return instruction.opcode == OpCodes.Stloc_0;
                case 1: return instruction.opcode == OpCodes.Stloc_1;
                case 2: return instruction.opcode == OpCodes.Stloc_2;
                case 3: return instruction.opcode == OpCodes.Stloc_3;
                default:
                    if (instruction.opcode == OpCodes.Stloc_S && instruction.operand is LocalBuilder builder)
                    {
                        return builder.LocalIndex == localIndex;
                    }
                    if (instruction.opcode == OpCodes.Stloc_S && instruction.operand is byte idx)
                    {
                        return idx == localIndex;
                    }
                    return false;
            }
        }

        private static CodeInstruction LoadLocal(int localIndex)
        {
            switch (localIndex)
            {
                case 0: return new CodeInstruction(OpCodes.Ldloc_0);
                case 1: return new CodeInstruction(OpCodes.Ldloc_1);
                case 2: return new CodeInstruction(OpCodes.Ldloc_2);
                case 3: return new CodeInstruction(OpCodes.Ldloc_3);
                default: return new CodeInstruction(OpCodes.Ldloc_S, (byte)localIndex);
            }
        }

        private static bool PreventUsingSpecificWood(Smelter smelter, Smelter.ItemConversion itemConversion)
        {
            if (smelter.m_name.Equals(SmelterDefinitions.KilnName) && ((SmelterConfiguration.dontProcessFineWood.Value && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(TillValhalla.WoodDefinitions.FineWoodName)) || (SmelterConfiguration.dontProcessRoundLog.Value && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(TillValhalla.WoodDefinitions.RoundLogName))))
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
            if (__instance.m_name.Equals(SmelterDefinitions.EitrRefineryName) && SmelterConfiguration.eitrRefineryIsEnabled.Value && SmelterConfiguration.eitrRefineryAutoDeposit.Value)
            {
                return spawn(helper.Clamp(SmelterConfiguration.eitrRefineryAutoRange.Value, 1f, 50f), false); // Configuration.Current.Windmill.ignorePrivateAreaCheck);
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
    //                return list;
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

            // Use cached smelter type instead of repeated string comparisons
            var smelterType = Smelter_Awake_Patch.GetCachedSmelterType(__instance);
            
            float autoRange = 0f;
            bool ignorePrivateAreaCheck = false;
            bool isKiln = false;

            switch (smelterType)
            {
                case Smelter_Awake_Patch.SmelterType.Kiln:
                    if (!SmelterConfiguration.kilnIsEnabled.Value || !SmelterConfiguration.kilnAutoFuel.Value)
                        return;
                    isKiln = true;
                    autoRange = SmelterConfiguration.kilnAutoRange.Value;
                    break;

                case Smelter_Awake_Patch.SmelterType.Smelter:
                    if (!SmelterConfiguration.smelterIsEnabled.Value || !SmelterConfiguration.smelterAutoFuel.Value)
                        return;
                    autoRange = SmelterConfiguration.smelterAutoRange.Value;
                    break;

                case Smelter_Awake_Patch.SmelterType.Furnace:
                    if (!SmelterConfiguration.furnaceIsEnabled.Value || !SmelterConfiguration.furnaceAutoFuel.Value)
                        return;
                    autoRange = SmelterConfiguration.furnaceAutoRange.Value;
                    break;

                case Smelter_Awake_Patch.SmelterType.Windmill:
                    if (!SmelterConfiguration.windmillIsEnabled.Value || !SmelterConfiguration.windmillAutoFuel.Value)
                        return;
                    autoRange = SmelterConfiguration.windmillAutoRange.Value;
                    break;

                case Smelter_Awake_Patch.SmelterType.SpinningWheel:
                    if (!SmelterConfiguration.spinningWheelIsEnabled.Value || !SmelterConfiguration.spinningWheelAutoFuel.Value)
                        return;
                    autoRange = SmelterConfiguration.eitrRefineryAutoRange.Value;
                    break;

                case Smelter_Awake_Patch.SmelterType.EitrRefinery:
                    if (!SmelterConfiguration.eitrRefineryIsEnabled.Value || !SmelterConfiguration.eitrRefineryAutoFuel.Value)
                        return;
                    autoRange = SmelterConfiguration.eitrRefineryAutoRange.Value;
                    break;

                default:
                    return;
            }

            float clampedRange = helper.Clamp(autoRange, 1f, 50f);
            int oreSpace = __instance.m_maxOre - __instance.GetQueueSize();
            int fuelSpace = __instance.m_maxFuel - (int)Math.Ceiling(__instance.GetFuel());

            // Handle fuel
            if ((bool)__instance.m_fuelItem && fuelSpace > 0)
            {
                ItemDrop.ItemData itemData = __instance.m_fuelItem.m_itemData;
                int num3 = InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(__instance.gameObject, clampedRange, itemData, fuelSpace, !ignorePrivateAreaCheck);
                for (int i = 0; i < num3; i++)
                {
                    InvokeAddFuelRpc(__instance);
                }
                if (num3 > 0 && Configuration.enableDebugLogging != null && Configuration.enableDebugLogging.Value)
                {
                    ZLog.Log("Added " + num3 + " fuel(" + itemData.m_shared.m_name + ") in " + __instance.m_name);
                }
            }

            if (oreSpace <= 0)
            {
                return;
            }

            // Get nearby chests once and reuse
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(__instance.gameObject, clampedRange);
            
            foreach (Container item in nearbyChests)
            {
                foreach (Smelter.ItemConversion item2 in __instance.m_conversion)
                {
                    if (isKiln)
                    {
                        if ((SmelterConfiguration.dontProcessFineWood.Value && item2.m_from.m_itemData.m_shared.m_name.Equals(TillValhalla.WoodDefinitions.FineWoodName)) || (SmelterConfiguration.dontProcessRoundLog.Value && item2.m_from.m_itemData.m_shared.m_name.Equals(TillValhalla.WoodDefinitions.RoundLogName)))
                        {
                            continue;
                        }
                        int threshold = SmelterConfiguration.kilnStopAutoFuelThreshold.Value >= 0 ? SmelterConfiguration.kilnStopAutoFuelThreshold.Value : 0;
                        if (threshold > 0 && InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(nearbyChests), item2.m_to.m_itemData) >= threshold)
                        {
                            return;
                        }
                    }
                    
                    ItemDrop.ItemData itemData2 = item2.m_from.m_itemData;
                    int num5 = InventoryAssistant.RemoveItemFromChest(item, itemData2, oreSpace);
                    
                    if (num5 > 0)
                    {
                        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(item2.m_from.gameObject.name);
                        
                        // TODO: Optimize by batching RPC calls instead of sending individual RPCs
                        for (int j = 0; j < num5; j++)
                        {
                            InvokeAddOreRpc(__instance, itemPrefab.name);
                        }
                        
                        oreSpace -= num5;
                        
                        if (num5 > 0 && Configuration.enableDebugLogging != null && Configuration.enableDebugLogging.Value)
                        {
                            ZLog.Log("Added " + num5 + " ores(" + itemData2.m_shared.m_name + ") in " + __instance.m_name);
                        }
                        
                        if (oreSpace == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private static readonly MethodInfo method_RPC_AddFuel = AccessTools.GetDeclaredMethods(typeof(Smelter)).FirstOrDefault(m => m.Name == "RPC_AddFuel");
        private static readonly MethodInfo method_RPC_AddOre = AccessTools.GetDeclaredMethods(typeof(Smelter)).FirstOrDefault(m => m.Name == "RPC_AddOre");

        private static void InvokeAddFuelRpc(Smelter smelter)
        {
            smelter.m_nview.InvokeRPC("RPC_AddFuel", BuildRpcPayload(method_RPC_AddFuel, null));
        }

        private static void InvokeAddOreRpc(Smelter smelter, string oreName)
        {
            smelter.m_nview.InvokeRPC("RPC_AddOre", BuildRpcPayload(method_RPC_AddOre, oreName));
        }

        private static object[] BuildRpcPayload(MethodInfo rpcMethod, string oreName)
        {
            if (rpcMethod == null)
            {
                return oreName == null ? new object[0] : new object[] { oreName };
            }

            List<object> payload = new List<object>();
            bool oreAssigned = false;

            foreach (ParameterInfo parameter in rpcMethod.GetParameters())
            {
                if (parameter.ParameterType == typeof(long))
                {
                    continue;
                }

                if (!oreAssigned && oreName != null && parameter.ParameterType == typeof(string))
                {
                    payload.Add(oreName);
                    oreAssigned = true;
                    continue;
                }

                payload.Add(GetDefaultRpcValue(parameter.ParameterType));
            }

            return payload.ToArray();
        }

        private static object GetDefaultRpcValue(Type parameterType)
        {
            if (parameterType == typeof(bool)) return false;
            if (parameterType == typeof(int)) return 1;
            if (parameterType == typeof(float)) return 1f;
            if (parameterType == typeof(string)) return string.Empty;

            return parameterType.IsValueType ? Activator.CreateInstance(parameterType) : null;
        }
    }
}

