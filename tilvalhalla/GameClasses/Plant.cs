//using HarmonyLib;
//using TillValhalla.Configurations.Sections;
//using TillValhalla.Configurations;

//namespace TillValhalla.GameClasses
//{


//    [HarmonyPatch(typeof(Plant), "HaveGrowSpace")]
//    public class Plant_HaveGrowSpace_Patch
//    {
//        public static bool Postfix(bool __result)
//        {
            
//                __result = !PlantConfiguration.needgrowspace.Value;
//                return __result;
            


//        }
//    }

//    [HarmonyPatch(typeof(Plant), nameof(Plant.Awake))]
//    public static class Plant_Awake_Patch
//    {
//        public static void Postfix(Plant __instance)
//        {
           
            
//                __instance.m_needCultivatedGround = PlantConfiguration.needcultivatedground.Value;
//                (__instance.GetComponentInParent(typeof(Piece)) as Piece).m_cultivatedGroundOnly = PlantConfiguration.needcultivatedground.Value;
            



//        }
//    }




//}