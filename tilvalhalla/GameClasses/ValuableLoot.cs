/*
using HarmonyLib;
using System;
using System.Collections.Generic;
using TillValhalla.Configurations;
using TillValhalla.Configurations.Sections;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.RPC_Pick))]
    public static class Pickable_ValuableLoot_Patch
    {
        private static readonly HashSet<string> ValuablePrefabNames = new HashSet<string>(StringComparer.Ordinal)
        {
            "Amber",
            "AmberPearl",
            "AmberPeral",
            "AncientCoin",
            "Coins",
            "Ruby"
        };

        private static string StripClone(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            int cloneIndex = name.IndexOf("(Clone)", StringComparison.Ordinal);
            return cloneIndex >= 0 ? name.Substring(0, cloneIndex) : name;
        }

        private static void Prefix(Pickable __instance)
        {
            string prefabName = __instance?.m_itemPrefab != null ? StripClone(__instance.m_itemPrefab.name) : "<null>";
            ZLog.Log($"[TillValhalla] Pickable.RPC_Pick fired. prefab='{prefabName}', currentAmount={__instance?.m_amount}, modEnabled={Configuration.modisenabled.Value}, valuableModifier={ItemDropConfiguration.ValuableItems.Value}");

            if (!Configuration.modisenabled.Value || ItemDropConfiguration.ValuableItems.Value == 0f)
            {
                return;
            }

            if (__instance?.m_itemPrefab == null || !ValuablePrefabNames.Contains(prefabName))
            {
                return;
            }

            // Prevent the game from re-scaling our modified amount, then apply the configured modifier.
            __instance.m_dontScale = true;

            int originalAmount = __instance.m_amount;
            int modifiedAmount = (int)Math.Round(helper.applyModifierValue(__instance.m_amount, ItemDropConfiguration.ValuableItems.Value));
            __instance.m_amount = Math.Max(1, modifiedAmount);

            ZLog.Log($"[TillValhalla] Applied valuable scaling to '{prefabName}': {originalAmount} -> {__instance.m_amount} (modifier={ItemDropConfiguration.ValuableItems.Value}).");
        }
    }
}
*/
