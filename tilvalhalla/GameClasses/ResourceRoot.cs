using HarmonyLib;
using TillValhalla;
using UnityEngine;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(ResourceRoot), nameof(ResourceRoot.Awake))]
    public class ResourceRoot_Awake_Patch
    {
        public static void Postfix(ResourceRoot __instance)
        {
            if (!SapCollectorConfiguration.enabled.Value && !Configuration.modisenabled.Value)
            {
                return;
            }
            else
            {
                __instance.m_maxLevel = SapCollectorConfiguration.rootmaxcapacity.Value;
                __instance.m_regenPerSec = SapCollectorConfiguration.rootregenpersec.Value;
            }

        }
    }

}