using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(CraftingStation), "Start")]
    public static class CraftingStation_Awake_Patch
    {
        public static void Postfix(CraftingStation __instance)
        {
            __instance.m_craftRequireRoof = CraftingStationConfiguration.craftingroofrequired.Value;
        }
    }
}