﻿using HarmonyLib;
using TillValhalla.Configurations.Sections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(Game), nameof(Game.UpdateRespawn))]
    public static class Player_UpdateRespawn_Transpiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //if (!Configuration.Current.Game.IsEnabled) return instructions;

            string firstarrival = GameConfiguration.firstspawnmessage.Value.ToString();

            List<CodeInstruction> il = instructions.ToList();
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Ldstr)
                {
                    il[i].operand = firstarrival;
                    return il.AsEnumerable();
                }
            }

            //ZLog.LogError("Failed to apply Game_GetPlayerDifficulty_Patch.Transpiler");

            return instructions;
        }
    }
	[HarmonyPatch(typeof(Location), "IsInsideNoBuildLocation")]
    public static class Location_IsInsideNoBuildLocation_Patch
    {
		public static bool Prefix(ref bool __result)
        {
            if (GameConfiguration.DisableBuildRestrictions.Value && GameConfiguration.enabled.Value)
            {
				__result = false;
				return false;
			}
            else
            {
                return true;
            }
		}
	}

}
