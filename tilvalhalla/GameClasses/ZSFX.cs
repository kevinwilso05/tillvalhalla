using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Jotunn;
using TillValhalla;
using TillValhalla.Configurations;
using TillValhalla.Configurations.Sections;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(ZSFX), "Awake")]
    public static class ZSFX_Awake_Patch
    {
        public static void Postfix(ZSFX __instance)
        {
            if (!Configuration.modisenabled.Value && !ZSFXConfiguration.enabled.Value)
            {
                return;
            }
            string zSFXName = GetZSFXName(__instance);
            if (ZSFXConfiguration.debugMode.Value)
            {
                Logger.LogInfo((object)("Check SFX:" + zSFXName));
            }
            Dictionary<string, Dictionary<string, AudioClip>> sFXList = global::TillValhalla.TillValhalla.SFXList;
            if (sFXList != null && sFXList.TryGetValue(zSFXName, out var value))
            {
                if (ZSFXConfiguration.debugMode.Value)
                {
                    Logger.LogInfo((object)("replacing SFX list by name: " + zSFXName));
                }
                __instance.m_audioClips = value.Values.ToArray();
            }
            else
            {
                if (global::TillValhalla.TillValhalla.SFX == null || __instance.m_audioClips == null)
                {
                    return;
                }
                for (int i = 0; i < __instance.m_audioClips.Length; i++)
                {
                    if (__instance.m_audioClips[i] == null)
                    {
                        continue;
                    }
                    if (ZSFXConfiguration.debugMode.Value)
                    {
                        Logger.LogInfo(("checking SFX: " + zSFXName + ", clip: " + (__instance.m_audioClips[i]).name));
                    }
                    if (global::TillValhalla.TillValhalla.SFX.TryGetValue((__instance.m_audioClips[i]).name, out var value2))
                    {
                        if (ZSFXConfiguration.debugMode.Value)
                        {
                            Logger.LogInfo(("replacing SFX: " + zSFXName + ", clip: " + (__instance.m_audioClips[i]).name));
                        }
                        __instance.m_audioClips[i] = value2;
                    }
                }
            }
        }

        public static string GetZSFXName(ZSFX zfx)
        {
            string name = ((Object)zfx).name;
            char[] anyOf = new char[2] { '(', ' ' };
            int num = name.IndexOfAny(anyOf);
            if (num != -1)
            {
                return name.Remove(num);
            }
            return name;
        }
    }
}


