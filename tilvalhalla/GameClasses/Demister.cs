using HarmonyLib;
using TillValhalla.Configurations.Sections;
using UnityEngine;

namespace TillValhalla.GameClasses
{
	//[HarmonyPatch(typeof(Demister), "Awake")]
	//public static class Demister_Awake_Patch
	//{
	//	private static void Postfix(ref ParticleSystemForceField ___m_forcefield)
	//	{
	//		___m_forcefield.endRange = DemisterConfiguration.endRange.Value;

	//	}
	//}
	//[HarmonyPatch(typeof(Piece), "Awake")]
	//public static class Piece_Awake_Patch
	//{
	//    public static void Postfix(Piece __instance)
	//    {
	//        if(__instance.m_craftingStation.m_name == "$piece_workbench" && !CraftingStationConfiguration.UpgradeSpaceRequirementEnabled.Value)
	//        {
	//            __instance.m_spaceRequirement = 0; 
	//        }

	//    }
	//}
}