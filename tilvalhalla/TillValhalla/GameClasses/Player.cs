using HarmonyLib;
using TillValhalla.Configurations.Sections;


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
        private static void PostFix(ref Player __instance)
        {
             //if (PlayerConfiguration.enabled.Value)
            //{
                __instance.m_maxCarryWeight = 600f;
                __instance.m_baseHP = PlayerConfiguration.basehp.Value;
                __instance.m_baseStamina = 300f;
            //}
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

    
}

