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

namespace HarmonyPatches
{

    [HarmonyPatch(typeof(Beehive), nameof(Beehive.Awake))]
    public class BeehivePatch
    {
       
        public static void Postfix(Beehive __instance)
        {
            __instance.m_maxHoney = 15;

        }
    }

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public class TeleportAllItems
    {
        public static void Postfix(ItemDrop __instance)
        {
            __instance.m_itemData.m_shared.m_teleportable = true;

        }
    }


}