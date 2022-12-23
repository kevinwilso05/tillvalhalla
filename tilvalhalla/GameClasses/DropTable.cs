using System;
using System.Collections.Generic;
using HarmonyLib;
using TillValhalla.Configurations.Sections;
using UnityEngine;
using Jotunn;
using Jotunn.Managers;

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

            int chitin = 0;
            GameObject chitinObject = null;




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
                    case "Chitin": // Chitin
                        chitin += 1;
                        chitinObject = toDrop;
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
            //Greydwarf eye
            for (int i = 0; i < helper.applyModifierValue(GreydwarfEye, gatherconfiguration.greydwarfeye.Value); i++)
            {
                defaultDrops.Add(GreydwarfEyeObject);
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

                var surtingcore = PrefabManager.Instance.GetPrefab("SurtlingCore");
                var greydwarf = PrefabManager.Instance.GetPrefab("Greydwarf").GetComponent<CharacterDrop>();
                var Greydwarf_Elite = PrefabManager.Instance.GetPrefab("Greydwarf_Elite").GetComponent<CharacterDrop>();
                var Greydwarf_Shaman = PrefabManager.Instance.GetPrefab("Greydwarf_Shaman").GetComponent<CharacterDrop>();


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


}