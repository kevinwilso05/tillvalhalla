using System;
using System.Collections.Generic;
using HarmonyLib;
using TillValhalla.Configurations.Sections;
using UnityEngine;
using Jotunn;
using Jotunn.Managers;
using JetBrains.Annotations;
using static CharacterDrop;
using static Player;
using static UnityEngine.UI.CanvasScaler;

namespace TillValhalla.GameClasses
{



    [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
    public static class DropTable_GetDropList_Patch
    {

        static float originalDropChance = 0;
        private static void Prefix(ref DropTable __instance, ref List<GameObject> __result, int amount)
        {
            originalDropChance = __instance.m_dropChance; // we have to save the original to change it back after the function
            if (gatherconfiguration.enabled.Value && gatherconfiguration.Dropchance.Value != 0)
            {
                float newDropChance = helper.applyModifierValue(__instance.m_dropChance, gatherconfiguration.Dropchance.Value);
                if (newDropChance >= 1)
                    newDropChance = 1;
                if (newDropChance <= 0)
                    newDropChance = 0;

                if (__instance.m_dropChance != 1)
                    __instance.m_dropChance = newDropChance;
            }
        }

        private static void Postfix(ref DropTable __instance, ref List<GameObject> __result, int amount)
        {
            __instance.m_dropChance = originalDropChance; // Apply the original drop chance in case modified

            if (!gatherconfiguration.enabled.Value)
                return;

            int wood = 0;
            GameObject woodObject = null;

            int coreWood = 0;
            GameObject coreWoodObject = null;

            int stone = 0;
            GameObject stoneObject = null;

            int scrapIron = 0;
            GameObject scrapIronObject = null;

            int tinOre = 0;
            GameObject tinOreObject = null;

            int copperOre = 0;
            GameObject copperOreObject = null;

            int silverOre = 0;
            GameObject silverOreObject = null;

            int elderBark = 0;
            GameObject elderBarkObject = null;

            int fineWood = 0;
            GameObject fineWoodObject = null;

            int yggdrasilWood = 0;
            GameObject yggdrasilWoodObject = null;

            int chitin = 0;
            GameObject chitinObject = null;

            int softTissue = 0;
            GameObject softTissueObject = null;

            int blackMarble = 0;
            GameObject blackMarbleObject = null;




            List<GameObject> defaultDrops = new List<GameObject>();
            foreach (GameObject toDrop in __result)
            {
                switch (toDrop.name)
                {
                    case "Wood": // Wood
                        wood += 1;
                        woodObject = toDrop;
                        break;
                    case "RoundLog": // Corewood
                        coreWood += 1;
                        coreWoodObject = toDrop;
                        break;
                    case "Stone": // Stone
                        stone += 1;
                        stoneObject = toDrop;
                        break;
                    case "IronScrap": // Iron
                        scrapIron += 1;
                        scrapIronObject = toDrop;
                        break;
                    case "TinOre": // Tin
                        tinOre += 1;
                        tinOreObject = toDrop;
                        break;
                    case "CopperOre": // Copper
                        copperOre += 1;
                        copperOreObject = toDrop;
                        break;
                    case "SilverOre": // Silver
                        silverOre += 1;
                        silverOreObject = toDrop;
                        break;
                    case "ElderBark": // ElderBark
                        elderBark += 1;
                        elderBarkObject = toDrop;
                        break;
                    case "FineWood": // Finewood
                        fineWood += 1;
                        fineWoodObject = toDrop;
                        break;
                    case "YggdrasilWood": // YggdrasilWood
                        yggdrasilWood += 1;
                        yggdrasilWoodObject = toDrop;
                        break;
                    case "SoftTissue": // SoftTissue
                        softTissue += 1;
                        softTissueObject = toDrop;
                        break;
                    case "BlackMarble": // BlackMarble
                        blackMarble += 1;
                        blackMarbleObject = toDrop;
                        break;

                    default:
                        defaultDrops.Add(toDrop);
                        break;
                }
            }

            // Add Wood
            for (int i = 0; i < helper.applyModifierValue(wood, gatherconfiguration.wood.Value); i++)
            {
                defaultDrops.Add(woodObject);
            }

            // Add CoreWood
            for (int i = 0; i < helper.applyModifierValue(coreWood, gatherconfiguration.coreWood.Value); i++)
            {
                defaultDrops.Add(coreWoodObject);
            }

            // Add Stone

            for (int i = 0; i < helper.applyModifierValue(stone, gatherconfiguration.stone.Value); i++)
            {
                defaultDrops.Add(stoneObject);
            }

            // ScrapIron
            for (int i = 0; i < helper.applyModifierValue(scrapIron, gatherconfiguration.ironScrap.Value); i++)
            {
                defaultDrops.Add(scrapIronObject);
            }

            // TinOre
            for (int i = 0; i < helper.applyModifierValue(tinOre, gatherconfiguration.tinOre.Value); i++)
            {
                defaultDrops.Add(tinOreObject);
            }

            // CopperOre
            for (int i = 0; i < helper.applyModifierValue(copperOre, gatherconfiguration.copperOre.Value); i++)
            {
                defaultDrops.Add(copperOreObject);
            }

            // silverOre
            for (int i = 0; i < helper.applyModifierValue(silverOre, gatherconfiguration.silverOre.Value); i++)
            {
                defaultDrops.Add(silverOreObject);
            }

            // ElderBark
            for (int i = 0; i < helper.applyModifierValue(elderBark, gatherconfiguration.elderBark.Value); i++)
            {
                defaultDrops.Add(elderBarkObject);
            }

            // FineWood
            for (int i = 0; i < helper.applyModifierValue(fineWood, gatherconfiguration.fineWood.Value); i++)
            {
                defaultDrops.Add(fineWoodObject);
            }

            // Chitin
            for (int i = 0; i < helper.applyModifierValue(chitin, gatherconfiguration.chitin.Value); i++)
            {
                defaultDrops.Add(chitinObject);
            }
            // YggdrasilWood
            for (int i = 0; i < helper.applyModifierValue(yggdrasilWood, gatherconfiguration.YggdrasilWood.Value); i++)
            {
                defaultDrops.Add(yggdrasilWoodObject);
            }
            for (int i = 0; i < helper.applyModifierValue(softTissue, gatherconfiguration.SoftTissue.Value); i++)
            {
                defaultDrops.Add(softTissueObject);
            }
            for (int i = 0; i < helper.applyModifierValue(blackMarble, gatherconfiguration.BlackMarble.Value); i++)
            {
                defaultDrops.Add(blackMarbleObject);
            }

            __result = defaultDrops;
        }
    }

    public class DropTableAdd
    {
        public static void surtingcoredropadd()
        {
            try
            {
                //var blackmetal = PrefabManager.Instance.GetPrefab("BlackMetalScrap");
                var surtingcore = PrefabManager.Instance.GetPrefab("SurtlingCore");
                var greydwarfeye = PrefabManager.Instance.GetPrefab("GreydwarfEye");
                var greydwarf = PrefabManager.Instance.GetPrefab("Greydwarf").GetComponent<CharacterDrop>();
                var Greydwarf_Elite = PrefabManager.Instance.GetPrefab("Greydwarf_Elite").GetComponent<CharacterDrop>();
                var Greydwarf_Shaman = PrefabManager.Instance.GetPrefab("Greydwarf_Shaman").GetComponent<CharacterDrop>();
                var greyling = PrefabManager.Instance.GetPrefab("Greyling").GetComponent<CharacterDrop>();


                greyling.m_drops.Add(new CharacterDrop.Drop
                {
                    m_amountMax = 3,
                    m_amountMin = 1,
                    m_chance = 40,
                    m_levelMultiplier = true,
                    m_onePerPlayer = false,
                    m_prefab = greydwarfeye
                });

                greydwarf.m_drops.Add(new CharacterDrop.Drop
                {
                    m_amountMax = 2,
                    m_amountMin = 0,
                    m_chance = 20,
                    m_levelMultiplier = true,
                    m_onePerPlayer = false,
                    m_prefab = surtingcore
                });

                Greydwarf_Elite.m_drops.Add(new CharacterDrop.Drop
                {
                    m_amountMax = 2,
                    m_amountMin = 1,
                    m_chance = 25,
                    m_levelMultiplier = true,
                    m_onePerPlayer = false,
                    m_prefab = surtingcore
                });

                Greydwarf_Shaman.m_drops.Add(new CharacterDrop.Drop
                {
                    m_amountMax = 2,
                    m_amountMin = 1,
                    m_chance = 20,
                    m_levelMultiplier = true,
                    m_onePerPlayer = false,
                    m_prefab = surtingcore
                });

            }
            catch
            {
                Jotunn.Logger.LogError($"Failed to load surtling drop tables");
            }
            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= surtingcoredropadd;
            }


        }
    }


    [HarmonyPatch(typeof(CharacterDrop), "GenerateDropList")]
    public static class Character_Drop_Add
    {


        [HarmonyPrefix]
        private static bool MultiplyLoot(CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
        {

            List<KeyValuePair<GameObject, int>> list = new List<KeyValuePair<GameObject, int>>();
            int num = ((!__instance.m_character) ? 1 : Mathf.Max(1, (int)Mathf.Pow(2f, __instance.m_character.GetLevel() - 1)));
            

            foreach (CharacterDrop.Drop drop in __instance.m_drops)
            {
                if (drop.m_prefab == null)
                {

                    continue;
                }
                float num2 = drop.m_chance;
                if (drop.m_levelMultiplier)
                {
                    num2 *= (float)num;
                    
                }
                if (!(UnityEngine.Random.value <= num2))
                {
                    continue;
                }
                int num3 = UnityEngine.Random.Range(drop.m_amountMin, drop.m_amountMax);
                if (drop.m_levelMultiplier)
                {
                    num3 *= num;
                }
                if (drop.m_onePerPlayer)
                {
                    num3 = ZNet.instance.GetNrOfPlayers();
                }
                if (num3 <= 0)
                {
                    continue;
                }

                if (!(drop.m_prefab != null))
                {
                    continue;
                }
                //foreach (string item in TillValhalla.whitelist)
                //{

                switch (drop.m_prefab.name)
                {

                    case "BlackMetalScrap": // BlackMarble
                        float blackMetalDrop = helper.applyModifierValue(num3, gatherconfiguration.BlackMetalScrap.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(blackMetalDrop)));
                        break;
                    case "GreydwarfEye": // BlackMarble
                        if (num3 == 0)
                        {
                            num3 = 1;
                        }
                        float GreydwarfEye = helper.applyModifierValue(num3, gatherconfiguration.GreydwarfEye.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(GreydwarfEye)));
                        break;
                    case "RawMeat": // BlackMarble
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float RawMeat = helper.applyModifierValue(num3, gatherconfiguration.RawMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(RawMeat)));
                        break;

                    case "DeerMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float DeerMeat = helper.applyModifierValue(num3, gatherconfiguration.DeerMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(DeerMeat)));
                        break;
                    case "HareMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float HareMeat = helper.applyModifierValue(num3, gatherconfiguration.HareMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(HareMeat)));
                        break;
                    case "LoxMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float LoxMeat = helper.applyModifierValue(num3, gatherconfiguration.LoxMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(LoxMeat)));
                        break;
                    case "NeckTail":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float NeckTail = helper.applyModifierValue(num3, gatherconfiguration.NeckTail.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(NeckTail)));
                        break;
                    case "BugMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float BugMeat = helper.applyModifierValue(num3, gatherconfiguration.BugMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(BugMeat)));
                        break;
                    case "SerpentMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float SerpentMeat = helper.applyModifierValue(num3, gatherconfiguration.SerpentMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(SerpentMeat)));
                        break;
                    case "WolfMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float WolfMeat = helper.applyModifierValue(num3, gatherconfiguration.WolfMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(WolfMeat)));
                        break;
                    case "ChickenMeat":
                        if (num3 == 0 && gatherconfiguration.FoodDropMinimumEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float ChickenMeat = helper.applyModifierValue(num3, gatherconfiguration.ChickenMeat.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(ChickenMeat)));
                        break;
                    case "DeerHide":
                        if (num3 == 0 && ItemDropConfiguration.HideAlwaysDropOneEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float DeerHide = helper.applyModifierValue(num3, ItemDropConfiguration.DeerHide.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(DeerHide)));
                        break;
                    case "ScaleHide":
                        if (num3 == 0 && ItemDropConfiguration.HideAlwaysDropOneEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float ScaleHide = helper.applyModifierValue(num3, ItemDropConfiguration.ScaleHide.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(ScaleHide)));
                        break;
                    case "LoxPelt":
                        if (num3 == 0 && ItemDropConfiguration.HideAlwaysDropOneEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float LoxPelt = helper.applyModifierValue(num3, ItemDropConfiguration.LoxPelt.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(LoxPelt)));
                        break;
                    case "WolfPelt":
                        if (num3 == 0 && ItemDropConfiguration.HideAlwaysDropOneEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float WolfPelt = helper.applyModifierValue(num3, ItemDropConfiguration.WolfPelt.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(WolfPelt)));
                        break;

                    case "LeatherScraps":
                        if (num3 == 0 && ItemDropConfiguration.HideAlwaysDropOneEnabled.Value)
                        {
                            num3 = 1;
                        }
                        float LeatherScraps = helper.applyModifierValue(num3, ItemDropConfiguration.LeatherScraps.Value);
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, (int)Math.Round(LeatherScraps)));
                        break;

                    default:
                        list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, num3));
                        break;

                }
                    

            }
            
            __result = list;
            return false;
        }
    }
}
