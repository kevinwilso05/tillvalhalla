//using HarmonyLib;



//namespace TillValhalla.GameClasses
//{

//    [HarmonyPatch(typeof(SEMan), "AddStatusEffect")]
//    public static class Player_UpdateEnvStatusEffects_Patch
//    {
//        private static void Postfix(SEMan __instance, ref StatusEffect statusEffect)
//        {
//            Player player = (Player)__instance.m_character;

//            if (EnvMan.instance.IsWet())
//            {
                
//                foreach(StatusEffect statusEffect2 in __instance.m_statusEffects)
//                {
//                    if (statusEffect2.m_name == "wet")
//                    {
//                        player.m_removeEffects.
//                    }
//                }
                
                
//                if (statusEffect.m_name == "wet")
//                {
//                    __instance.m_statusEffects = null;
//                    player.m_seman.m_statusEffects = null;
//                }
//            }

//        }
//    }
//}