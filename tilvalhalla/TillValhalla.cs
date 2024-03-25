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
using BepInEx.Configuration;
using System.Linq;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.Remoting.Contexts;
using System.Globalization;
using BepInEx.Logging;

namespace TillValhalla
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]

    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class TillValhalla : BaseUnityPlugin
    {
        public const string PluginGUID = "kwilson.TillValhalla";
        public const string PluginName = "TillValhalla";
        public const string PluginVersion = "2.4.9";

        public readonly Harmony _harmony = new Harmony(PluginGUID);

        //loading assets and prefabs

        private AssetBundle undestructablewallbundle;
        private AssetBundle testspritebundle;

		private static TillValhalla context;
		//Loading Textures
		private Texture2D Textureprefab;

        //Loading Sprites
        private Sprite wings;
        private Sprite varpaint1;
		
        public static class WoodDefinitions
		{
			public static readonly string FineWoodName = "$item_finewood";

			public static readonly string RoundLogName = "$item_roundlog";
		}


		//private GameObject

		// Your mod's custom localization
		private CustomLocalization Localization;
        public static List<string> whitelist = new List<string> {};
		//JSON Files
		//public static string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		//public static string DropModifications = Path.Combine(assemblyFolder, "Assets/DropConfig.json");
		//public static string DropJsonText = File.ReadAllText(DropModifications);
		

		//public static List<DropModifier> dropModifiers = JsonConvert.DeserializeObject<List<DropModifier>>(TillValhalla.DropJsonText);


		private void Awake()
        {
			context = this;
			//Load Game Config
			Configuration.Awake(this);
            Logger.LogInfo("Loaded Game Configuration");

            if (Configuration.modisenabled.Value != true)
            {
                
                Logger.LogInfo("Error while loading configuration file.");
            }
            else
            {
                Logger.LogInfo("Configuration file successfully loaded");

				// Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
				// https://github.com/MonoMod/MonoMod


                // To learn more about Jotunn's features, go to
                // https://valheim-modding.github.io/Jotunn/tutorials/overview.html

                LoadConfigs();
                _harmony.PatchAll();
                //LoadAssets();
                AddLocalizations();
                //AddItemsandprefabs();
                 
                // Add custom items cloned from vanilla items
                PrefabManager.OnVanillaPrefabsAvailable += ModifyVanillaItems;
                PrefabManager.OnVanillaPrefabsAvailable += DropTableAdd.surtingcoredropadd;
				PrefabManager.OnVanillaPrefabsAvailable += AddItemsandprefabs;
			}
			

		}
		private void Update()
		{
			if (Configuration.modisenabled.Value && !HotBarUtils.IgnoreKeyPresses(true) && HotBarUtils.CheckKeyDown(inventoryconfiguration.hotKey.Value))
			{
				int gridHeight = Player.m_localPlayer.GetInventory().GetHeight();
				int rows = Math.Max(1, Math.Min(gridHeight, inventoryconfiguration.rowsToSwitch.Value));

				List<ItemDrop.ItemData> items = Traverse.Create(Player.m_localPlayer.GetInventory()).Field("m_inventory").GetValue<List<ItemDrop.ItemData>>();
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i].m_gridPos.y >= rows)
						continue;
					items[i].m_gridPos.y--;
					if (items[i].m_gridPos.y < 0)
						items[i].m_gridPos.y = rows - 1;
				}
				Traverse.Create(Player.m_localPlayer.GetInventory()).Method("Changed").GetValue();
			}
		}
        [HarmonyPatch(typeof(Terminal), "InputText")]
        static class InputText_Patch
        {
            static bool Prefix(Terminal __instance)
            {
                if (!Configuration.modisenabled.Value)
                    return true;
                string text = __instance.m_input.text;
                if (text.ToLower().Equals("hotbarswitch reset"))
                {
                    context.Config.Reload();
                    context.Config.Save();
                    Traverse.Create(__instance).Method("AddString", new object[] { text }).GetValue();
                    Traverse.Create(__instance).Method("AddString", new object[] { "hotbar switch config reloaded" }).GetValue();
                    return false;
                }
                return true;
            }
        }



        private void AddLocalizations()
        {
            Localization = LocalizationManager.Instance.GetLocalization();
            Localization.AddTranslation("English", new Dictionary<string, string>
                {
            {"fire_wood_chest_name", "Firewood Chest" },
			{"fire_wood_chest_description", "Chest for storing wood for the fires to pull from."},
	        });


        }



        private void AddItemsandprefabs()
        {
            //1
            //load indestructable wall and add to the hammer piece table
            PieceConfig FireWoodChest = new PieceConfig(); 
            FireWoodChest.Name = "$fire_wood_chest_name";
            FireWoodChest.Description = "$fire_wood_chest_description";
            FireWoodChest.PieceTable = "Hammer";
            FireWoodChest.Category = "Crafting";
            PieceManager.Instance.AddPiece(new CustomPiece("firewood_chest", "piece_chest_wood", FireWoodChest));
            PieceManager.Instance.GetPiece("firewood_chest").PiecePrefab.GetComponent<Container>().m_name = "$fire_wood_chest_name"; 
            PieceManager.Instance.GetPiece("firewood_chest").PiecePrefab.GetComponent<Transform>().localScale = new Vector3(.5f, .5f, .5f);
			PrefabManager.OnVanillaPrefabsAvailable -= AddItemsandprefabs;

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
                Logger.LogError("Failed to load Game Configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Game Configuration");
            }
            //BeehiveConfiguration
            try
            {
                BeehiveConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load beehive configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Beehive Configuration");
            }
            //ItemDropConfiguration
            try
            {
                ItemDropConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load ItemDrop configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded ItemDrop Configuration");
            }
            //InventoryConfiguration
            try
            {
                inventoryconfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Inventory Configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Inventory Configuration");
            }
            //ShipConfiguration
            try
            {
                ShipConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Ship Configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Ship Configuration");
            }
            //CraftingStationConfiguration
            try
            {
                CraftingStationConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load CraftingStation configuration");
            }
            finally
            {
                Logger.LogInfo("Loaded CraftingStation Configuration");
            }
            //GatherConfiguration
            try
            {
                gatherconfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Gather configuration");
            }
            finally
            {
                Logger.LogInfo("Loaded Gather Configuration");
            }
            //FireplaceConfiguration
            try
            {
				FireplaceConfiguration.Awake(this);
			}
			catch
            {
				Logger.LogError("Failed to load Fireplace configuration");
			}
			finally
            {
				Logger.LogInfo("Loaded Fireplace Configuration");
			}
            //PlayerConfiguration
            try
            {
                PlayerConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Player configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Player Configuration");
            }
            //PlantConfiguration
            try
            {
                PlantConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Plant configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Plant Configuration");
            }
            //ContainerConfiguration
            try
            {
                containerconfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Container configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Container Configuration");
            }
            //Smelter Configuration 
            try
            {
                SmelterConfiguration.Awake(this);
            }
            catch
            {

                Logger.LogError("Failed to load Smelter configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Smelter Configuration");
            }
            try
            {
                SapCollectorConfiguration.Awake(this);
            }
            catch
            {
                Logger.LogError("Failed to load Sap collection configuration");
            }
            finally
            {
				Logger.LogInfo("Loaded Sap Collection Configuration");
			}

            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Logger.LogInfo("Initial Config sync event received");
                   
                }
                else
                {
					Logger.LogInfo("Config sync event received");
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
                //Updatename for FirewoodChest

				//var boar = PrefabManager.Instance.GetPrefab
				//add ValhallaShieldWood

				//    Sprite var1 = wings;
				//    Sprite var2 = varpaint1;
				//    Texture2D styleTex = Textureprefab;
				//    CustomItem ValhallaShieldWood = new CustomItem("ValhallaShieldWood", "ShieldWood", new ItemConfig
				//    {
				//        Name = "$valhallashieldwood",
				//        Description = "$valhallashieldwood_description",
				//        Requirements = new RequirementConfig[]
				//{
				//    new RequirementConfig{ Item = "Wood", Amount = 1 }
				//},
				//        Icons = new[]
				//           {
				//            var1, var2
				//        },
				//        StyleTex = styleTex
				//    });
				//    ItemManager.Instance.AddItem(ValhallaShieldWood);

				//    //Add Valhalla Shield Banded

				//    CustomItem ValhallaShieldBanded = new CustomItem("ValhallaShieldBanded", "ShieldBanded", new ItemConfig
				//    {
				//        Name = "$valhallaShieldBanded",
				//        Description = "$valhallaShieldBanded_description",
				//        Requirements = new RequirementConfig[]
				//{
				//    new RequirementConfig{ Item = "Wood", Amount = 1}
				//},
				//        Icons = new[]
				//           {
				//            var1, var2
				//        },
				//        StyleTex = styleTex
				//    });
				//    ItemManager.Instance.AddItem(ValhallaShieldBanded);

				//    //Add Valhalla Shield Black Metal

				//    CustomItem ValhallaShieldBlackmetal = new CustomItem("ValhallaShieldBlackmetal", "ShieldBlackmetal", new ItemConfig
				//    {
				//        Name = "$valhallaShieldBlackmetal",
				//        Description = "$valhallaShieldBlackmetal_description",
				//        Requirements = new RequirementConfig[]
				//{
				//    new RequirementConfig{ Item = "Wood", Amount = 1 }
				//},
				//        Icons = new[]
				//           {
				//            var1, var2
				//        },
				//        StyleTex = styleTex
				//    });
				//    ItemManager.Instance.AddItem(ValhallaShieldBlackmetal);

				//    //New Cape

				//    CustomItem ValhallaCapeTrollHide = new CustomItem("ValhallaCapeTrollHide", "CapeTrollHide", new ItemConfig
				//    {
				//        Name = "$valhallaCapeTrollHide",
				//        Description = "$valhallaCapeTrollHide_description",
				//        Requirements = new RequirementConfig[]
				//{
				//    new RequirementConfig{ Item = "Wood", Amount = 1 }
				//},
				//        Icons = new[]
				//           {
				//            var1, var2
				//        },
				//        StyleTex = styleTex
				//    });
				//    ItemManager.Instance.AddItem(ValhallaCapeTrollHide);

			}

            catch (Exception ex)
            {
                Logger.LogError($"Error while adding variant item: {ex}");
            }

            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= ModifyVanillaItems;
            }

        }


    }
}


