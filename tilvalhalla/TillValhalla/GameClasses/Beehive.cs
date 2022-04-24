using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(Beehive), "Awake")]
    public static class Beehive_Awake_Patch
    {
        private static void Postfix(Beehive __instance)
        {
            if (BeehiveConfiguration.enabled.Value && GameConfiguration.isenabled.Value)
            {
                __instance.m_maxHoney = BeehiveConfiguration.beehivemaxhoney.Value;
                __instance.m_secPerUnit = BeehiveConfiguration.beehiveHoneyProductionSpeed.Value;
            }

        }
    }
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake_Patch
    {
        private static void Prefix(Player __instance)
        {
            //if (PlayerConfiguration.enabled.Value)
            //{
            __instance.m_maxCarryWeight = 600f;
            __instance.m_baseHP = PlayerConfiguration.basehp.Value;
            __instance.m_baseStamina = 300f;
            //}
        }
    }

}