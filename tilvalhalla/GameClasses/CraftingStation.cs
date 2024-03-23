//using HarmonyLib;
//using TillValhalla.Configurations.Sections;
//using TillValhalla.Configurations;

//namespace TillValhalla.GameClasses
//{
//    [HarmonyPatch(typeof(CraftingStation), "Start")]
//    public static class CraftingStation_Awake_Patch
//    {
//        public static void Postfix(CraftingStation __instance)
//        {
//            __instance.m_craftRequireRoof = CraftingStationConfiguration.craftingroofrequired.Value;
//            __instance.m_rangeBuild = CraftingStationConfiguration.workbenchcraftingRange.Value; 
            
//        }
//    }
//    //[HarmonyPatch(typeof(Piece), "Awake")]
//    //public static class Piece_Awake_Patch
//    //{
//    //    public static void Postfix(Piece __instance)
//    //    {
//    //        if(__instance.m_craftingStation.m_name == "$piece_workbench" && !CraftingStationConfiguration.UpgradeSpaceRequirementEnabled.Value)
//    //        {
//    //            __instance.m_spaceRequirement = 0; 
//    //        }

//    //    }
//    //}
//}