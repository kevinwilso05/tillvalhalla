//using HarmonyLib;
//using TillValhalla.Configurations.Sections;
//using TillValhalla.Configurations;

//namespace TillValhalla.GameClasses
//{

//    [HarmonyPatch(typeof(Beehive), "Awake")]
//    public static class Beehive_Awake_Patch
//    {
//        private static void Postfix(Beehive __instance)
//        {
//            if (BeehiveConfiguration.enabled.Value && Configuration.modisenabled.Value)
//            {
//                __instance.m_maxHoney = BeehiveConfiguration.beehivemaxhoney.Value;
//                __instance.m_secPerUnit = BeehiveConfiguration.beehiveHoneyProductionSpeed.Value;
//            }

//        }
//    }
    
//}