using HarmonyLib;
using TillValhalla;
using UnityEngine;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(SapCollector), nameof(SapCollector.Awake))]
    public class SapCollector_Awake_Patch
    {
        public static void Postfix(SapCollector __instance)
        {
            if (!SapCollectorConfiguration.enabled.Value && !Configuration.modisenabled.Value)
            {
                return;
            }
            else
            {
                __instance.m_secPerUnit = SapCollectorConfiguration.collectorsecperunit.Value;
                __instance.m_maxLevel = SapCollectorConfiguration.collectormaxcapacity.Value;
                
            }


        }
    }

}