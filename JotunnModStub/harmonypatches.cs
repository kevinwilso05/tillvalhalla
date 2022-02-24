using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.GUI;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using UnityEngine.UI;
using Logger = Jotunn.Logger;

namespace honeyaddition
{
    [HarmonyPatch]

    internal class hivegalore : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("kwilson.ValheimMod");

        void Awake()
        {
            harmony.PatchAll();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Beehive), "Awake")]
        public static void BeehiveAwake_Patch(Beehive __instance)
        {
            __instance.m_maxHoney = 15;

        }
    }

}
}