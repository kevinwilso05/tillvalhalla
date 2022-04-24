// TillValhalla
// a Valheim mod skeleton using Jötunn
// 
// File:    TillValhalla.cs
// Project: TillValhalla

using BepInEx;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Logger = Jotunn.Logger;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;
using TillValhalla.GameClasses;

namespace TillValhalla
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]

    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class TillValhalla : BaseUnityPlugin
    {
        public const string PluginGUID = "kwilson.TillValhalla";
        public const string PluginName = "TillValhalla";
        public const string PluginVersion = "2.2.0";

        public readonly Harmony _harmony = new Harmony(PluginGUID);

        //loading assets and prefabs

        private AssetBundle undestructablewallbundle;
        private AssetBundle testspritebundle;


        //Loading Textures
        private Texture2D Textureprefab;

        //Loading Sprites
        private Sprite wings;
        private Sprite varpaint1;

        //private GameObject

        // Your mod's custom localization
        private CustomLocalization Localization;

        

        private void Awake()
        {
            
            //Load Game Config
            Configuration.Awake(this);
            Jotunn.Logger.LogMessage("Loaded Game Configuration");

            if (Configuration.modisenabled.Value != true)
            {
                Jotunn.Logger.LogMessage("Error while loading configuration file.");
            }
            else
            {
                Jotunn.Logger.LogMessage("Configuration file successfully loaded");
                
                // Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
                // https://github.com/MonoMod/MonoMod
               

                // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
                Jotunn.Logger.LogInfo("ModStub has landed");

                // To learn more about Jotunn's features, go to
                // https://valheim-modding.github.io/Jotunn/tutorials/overview.html

                LoadConfigs();
                _harmony.PatchAll();
                LoadAssets();
                AddLocalizations();
                AddItemsandprefabs();
                //DropTableAdd.surtingcoredropadd();


                // Add custom items cloned from vanilla items
                PrefabManager.OnVanillaPrefabsAvailable += ModifyVanillaItems;
                PrefabManager.OnVanillaPrefabsAvailable += DropTableAdd.surtingcoredropadd;
            }
            
            

            
            

        }

        

        private void LoadAssets()
        {
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(TillValhalla).Assembly.GetManifestResourceNames())}");

            //loadprefabbundles
            try
            {
                //Load Resource Bundle
                undestructablewallbundle = AssetUtils.LoadAssetBundleFromResources("undestructablewall", typeof(TillValhalla).Assembly);
            }
            catch
            {
                Jotunn.Logger.LogError($"Failed to load asset bundle: {undestructablewallbundle}");
                
            }
            finally
            {
                Jotunn.Logger.LogInfo($"Loaded asset bundle: {undestructablewallbundle}");
            }


            //Load testspritebundle
            try
            {
                //LoadResourceBundle
                testspritebundle = AssetUtils.LoadAssetBundleFromResources("testspritebundle", typeof(TillValhalla).Assembly);
                
                //LoadTexture2D
                Textureprefab = testspritebundle.LoadAsset<Texture2D>("test_texturesheet.png");

                //LoadSprites

                wings = testspritebundle.LoadAsset<Sprite>("wings.png");
                varpaint1 = testspritebundle.LoadAsset<Sprite>("test_var1.png");
            }
            catch
            {
                Jotunn.Logger.LogError($"Failed to load asset bundle: {testspritebundle}");

            }
            finally
            {
                Jotunn.Logger.LogInfo($"Loaded asset bundle: {testspritebundle}");
            }

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
            //1
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

        

        private void LoadConfigs()
        {
            //GameConfiguration
            try
            {
                GameConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Game Configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Game Configuration");
            }
            //BeehiveConfiguration
            try
            {
                BeehiveConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load beehive configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Beehive Configuration");
            }
            //ItemDropConfiguration
            try
            {
                ItemDropConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load ItemDrop configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded ItemDrop Configuration");
            }
            //InventoryConfiguration
            try
            {
                inventoryconfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Inventory Configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Inventory Configuration");
            }
            //ShipConfiguration
            try
            {
                ShipConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Ship Configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Ship Configuration");
            }
            //CraftingStationConfiguration
            try
            {
                CraftingStationConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load CraftingStation configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded CraftingStation Configuration");
            }
            //GatherConfiguration
            try
            {
                gatherconfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Gather configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Gather Configuration");
            }
            //PlayerConfiguration
            try
            {
                PlayerConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Player configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Player Configuration");
            }
            //PlantConfiguration
            try
            {
                PlantConfiguration.Awake(this);
            }
            catch
            {
                Jotunn.Logger.LogError("Failed to load Plant configuration");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Loaded Plant Configuration");
            }


            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                }
                else
                {
                    Jotunn.Logger.LogMessage("Config sync event received");
                }
            };
        }


        private void ModifyVanillaItems()
        {
            try
            {

                //Remove Cheat Sword Functionality
                var getswordprefab = PrefabManager.Instance.GetPrefab("SwordCheat");
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_slash = 0;
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_chop = 0;
                getswordprefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_pickaxe = 0;

                //add ValhallaShieldWood

                Sprite var1 = wings;
                Sprite var2 = varpaint1;
                Texture2D styleTex = Textureprefab;
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
                new RequirementConfig{ Item = "Wood", Amount = 1}
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


