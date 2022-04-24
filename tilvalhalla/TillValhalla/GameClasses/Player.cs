using HarmonyLib;
using TillValhalla.Configurations.Sections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System;


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

    [HarmonyPatch(typeof(Game), nameof(Game.UpdateRespawn))]
    public static class Player_UpdateRespawn_Transpiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //if (!Configuration.Current.Game.IsEnabled) return instructions;

            string firstarrival = "I have cum";

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
}

