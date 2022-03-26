using HarmonyLib;



namespace TillValhalla.GameClasses
{

    //No wet debuff
    [HarmonyPatch(typeof(EnvMan), "IsWet")]
    internal class EnvMan_Patch
    {
        public static bool Postfix(bool __result)
        {


            __result = false;
            return __result;

        }
    }
}

