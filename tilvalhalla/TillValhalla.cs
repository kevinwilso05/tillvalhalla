// TillValhalla
// a Valheim mod skeleton using Jötunn
// 
// File:    TillValhalla.cs
// Project: TillValhalla

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

namespace TillValhalla
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]

    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class kwilsonvalheimmodv2 : BaseUnityPlugin
    {
        public const string PluginGUID = "kwilson.TillValhalla";
        public const string PluginName = "TillValhalla";
        public const string PluginVersion = "1.0.1";

        public readonly Harmony _harmony = new Harmony(PluginGUID);

        //private readonly Harmony _harmony = new Harmony("kwilson.valheimmodv2");

        //loading assets and prefabs

        private AssetBundle undestructablewallbundle;
        private AssetBundle SteelIngotBundle;
        //private GameObject

        // Your mod's custom localization
        private CustomLocalization Localization;



        private void Awake()
        {
            // Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
            // https://github.com/MonoMod/MonoMod
            On.FejdStartup.Awake += FejdStartup_Awake;

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html


            _harmony.PatchAll();
            LoadAssets();
            AddLocalizations();
            AddItemsandprefabs();


            // Add custom items cloned from vanilla items
            PrefabManager.OnVanillaPrefabsAvailable += ModifyVanillaItems;


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

        private void LoadAssets()
        {
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(kwilsonvalheimmodv2).Assembly.GetManifestResourceNames())}");
            SteelIngotBundle = AssetUtils.LoadAssetBundleFromResources("steel", typeof(kwilsonvalheimmodv2).Assembly);
            undestructablewallbundle = AssetUtils.LoadAssetBundleFromResources("undestructablewall", typeof(kwilsonvalheimmodv2).Assembly);
            Jotunn.Logger.LogInfo($"Loaded asset bundle: {undestructablewallbundle}");
            Jotunn.Logger.LogInfo($"Loaded asset bundle: {SteelIngotBundle }");

        }


        private void AddLocalizations()
        {
            // Create a custom Localization instance and add it to the Manager
            Localization = new CustomLocalization();
            LocalizationManager.Instance.AddLocalization(Localization);

            // Add translations for custom items
            Localization.AddTranslation("English", new Dictionary<string, string>
    {
        {"valhallashieldwood", "Valhalla Wood Shield" }, {"valhallashieldwood_description", "Wooden shield of all of the fun!" }

    });


        }


        private void AddItemsandprefabs()
        {

            //load indestructable wall and add to the hammer piece table
            var makebp_prefab = undestructablewallbundle.LoadAsset<GameObject>("undestructablewoodwall");
            var makebp = new CustomPiece(makebp_prefab, fixReference: false,
                new PieceConfig
                {
                    PieceTable = "Hammer",
                    Category = "Building"

                });
            PieceManager.Instance.AddPiece(makebp);




        }


        private void ModifyVanillaItems()
        {
            try
            {

                //cheatsword test
                var getswordprefab = PrefabManager.Instance.GetPrefab("SwordCheat");
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_slash = 0;
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_chop = 0;
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_pickaxe = 0;

                //add ValhallaShieldWood

                Sprite var1 = AssetUtils.LoadSpriteFromFile("TillValhalla/Assets/wings.png");
                Sprite var2 = AssetUtils.LoadSpriteFromFile("TillValhalla/Assets/test_var1.png");
                Texture2D styleTex = AssetUtils.LoadTexture("TillValhalla/Assets/test_texturesheet.png");
                CustomItem ValhallaShieldWood = new CustomItem("ValhallaShieldWood", "ShieldWood", new ItemConfig
                {
                    Name = "$valhallashieldwood",
                    Description = "$valhallashieldwood_description",
                    Requirements = new RequirementConfig[]
            {
                new RequirementConfig{ Item = "Wood", Amount = 1 }
            },
                    Icons = new[]
                       {
                        var1, var2
                    },
                    StyleTex = styleTex
                });
                ItemManager.Instance.AddItem(ValhallaShieldWood);

                //Add Valhalla Shield Banded

                CustomItem ValhallaShieldBanded = new CustomItem("ValhallaShieldBanded", "ShieldBanded", new ItemConfig
                {
                    Name = "$valhallaShieldBanded",
                    Description = "$valhallaShieldBanded_description",
                    Requirements = new RequirementConfig[]
            {
                new RequirementConfig{ Item = "Wood", Amount = 1 }
            },
                    Icons = new[]
                       {
                        var1, var2
                    },
                    StyleTex = styleTex
                });
                ItemManager.Instance.AddItem(ValhallaShieldBanded);

                //Add Valhalla Shield Black Metal

                CustomItem ValhallaShieldBlackmetal = new CustomItem("ValhallaShieldBlackmetal", "ShieldBlackmetal", new ItemConfig
                {
                    Name = "$valhallaShieldBlackmetal",
                    Description = "$valhallaShieldBlackmetal_description",
                    Requirements = new RequirementConfig[]
            {
                new RequirementConfig{ Item = "Wood", Amount = 1 }
            },
                    Icons = new[]
                       {
                        var1, var2
                    },
                    StyleTex = styleTex
                });
                ItemManager.Instance.AddItem(ValhallaShieldBlackmetal);

                //New Cape

                CustomItem ValhallaCapeTrollHide = new CustomItem("ValhallaCapeTrollHide", "CapeTrollHide", new ItemConfig
                {
                    Name = "$valhallaCapeTrollHide",
                    Description = "$valhallaCapeTrollHide_description",
                    Requirements = new RequirementConfig[]
            {
                new RequirementConfig{ Item = "Wood", Amount = 1 }
            },
                    Icons = new[]
                       {
                        var1, var2
                    },
                    StyleTex = styleTex
                });
                ItemManager.Instance.AddItem(ValhallaCapeTrollHide);

            }

            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while adding variant item: {ex}");
            }

            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= ModifyVanillaItems;
            }

        }


    }


}


