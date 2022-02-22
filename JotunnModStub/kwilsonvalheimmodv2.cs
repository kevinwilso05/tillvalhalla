// JotunnModStub
// a Valheim mod skeleton using Jötunn
// 
// File:    JotunnModStub.cs
// Project: JotunnModStub

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
using UnityEngine.UI;
using Logger = Jotunn.Logger;

namespace kwilsonvalheimmodv2
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class kwilsonvalheimmodv2 : BaseUnityPlugin
    {
        public const string PluginGUID = "kwilson.valheimmodv2";
        public const string PluginName = "kwilsonvalheimmodv2";
        public const string PluginVersion = "1.0.0";

        //loading assets and prefabs

        private AssetBundle undestructablewallbundle;
        //private GameObject


        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            // Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
            // https://github.com/MonoMod/MonoMod
            On.FejdStartup.Awake += FejdStartup_Awake;
            
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html


            AddItemsWithConfigs();
            


        }

        private void FejdStartup_Awake(On.FejdStartup.orig_Awake orig, FejdStartup self)
        {
            // This code runs before Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup is going to awake");

            // Call this method so the original game method is invoked
            orig(self);

            // This code runs after Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup has awoken");
        }

        

        private void AddItemsWithConfigs()
        {
            // Load asset bundle from the filesystem
            undestructablewallbundle = AssetUtils.LoadAssetBundle("JotunnModStub/Assets/undestructablewall");
            Jotunn.Logger.LogInfo($"Loaded asset bundle: {undestructablewallbundle}");

            // Load and add all our custom stuff to Jötunn
            CreatewallPieces();


            // Don't forget to unload the bundle to free the resources
            undestructablewallbundle.Unload(false);
        }


        private void CreatewallPieces()
        {
            var makebp_prefab = undestructablewallbundle.LoadAsset<GameObject>("undestructablewoodwall");
            var makebp = new CustomPiece(makebp_prefab, fixReference: false,
                new PieceConfig
                {
                    PieceTable = "Building"

                });
            PieceManager.Instance.AddPiece(makebp);

            

            
        }


    }
}