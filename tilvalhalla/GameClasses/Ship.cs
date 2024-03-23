//using HarmonyLib;
//using TillValhalla.Configurations.Sections;
//using TillValhalla.Configurations;

//namespace TillValhalla.GameClasses
//{
//    [HarmonyPatch(typeof(Ship), "Awake")]
//    public static class Ship_Awake_Patch
//    {
//        public static void Postfix(Ship __instance)
//        {
//            __instance.m_stearForce = ShipConfiguration.stearforce.Value;
//            __instance.m_rudderSpeed = ShipConfiguration.rudderspeed.Value;
//            __instance.m_sailForceFactor = ShipConfiguration.sailforcefactor.Value;
//        }
//    }
//}