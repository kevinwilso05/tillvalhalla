using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class noItemTeleportPrevention
    {
        public static void Postfix(ItemDrop __instance)
        {
            __instance.m_itemData.m_shared.m_teleportable = true;

        }
    }
}