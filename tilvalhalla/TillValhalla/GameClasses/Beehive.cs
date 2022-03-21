using HarmonyLib;
using Logger = Jotunn.Logger;
using TillValhalla.Configurations.Sections;

namespace TillValhalla.GameClasses
{

    [HarmonyPatch(typeof(Beehive), nameof(Beehive.Awake))]
    public class BeehivePatch
    {

        public static void Postfix(Beehive __instance)
        {
            if (BeehiveConfiguration.enabled.Value)
            {
                __instance.m_maxHoney = BeehiveConfiguration.beehivemaxhoney.Value;
                __instance.m_secPerUnit = BeehiveConfiguration.beehiveHoneyProductionSpeed.Value;
            }
           




        }
    }




}