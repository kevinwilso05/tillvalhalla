using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TillValhalla.Configurations.Sections;


namespace TillValhalla.GameClasses
{
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
}