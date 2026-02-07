using HarmonyLib;
using TillValhalla.Configurations.Sections;
using TillValhalla.Configurations;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TillValhalla.GameClasses;
using System.Linq;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
//using static MonoMod.InlineRT.MonoModRule;

namespace TillValhalla.GameClasses
{
	public static class FirePlaceDefinitions
	{
		public static readonly string FirePitName = "$piece_fire";

		public static readonly string BonfireName = "$piece_bonfire";

		public static readonly string HearthName = "$piece_hearth";
        //public static readonly string OvenName = "$piece_oven";

    }
	[HarmonyPatch(typeof(Fireplace), "Awake")]
	public static class Fireplace_Awake_Patch
	{
		private static void Postfix(Fireplace __instance)
		{
			if (__instance.m_name.Contains("torch"))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.TorchInfiniteFuel.Value;
			}
			else if (__instance.m_name.Contains("sconce"))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.TorchInfiniteFuel.Value;
			}
			else if (__instance.m_name.Contains("Brazier"))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.BrazierInfiniteFuel.Value;
			}
			else if (__instance.m_name.Equals(FirePlaceDefinitions.FirePitName))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.FireplaceInfiniteFuel.Value;
			}
			else if (__instance.m_name.Equals(FirePlaceDefinitions.HearthName))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.FireplaceInfiniteFuel.Value;
			}
			else if (__instance.m_name.Equals(FirePlaceDefinitions.BonfireName))
			{
				__instance.m_infiniteFuel = FireplaceConfiguration.FireplaceInfiniteFuel.Value;
			}
		}
	}

	[HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
	public static class Fireplace_UpdateSmelter_Patch
	{
		private static void Prefix(Fireplace __instance)
		{
			if (__instance == null || !Player.m_localPlayer || __instance.m_nview == null || !__instance.m_nview.IsOwner())
			{
				return;
			}
			Stopwatch stopwatch = GameObjectAssistant.GetStopwatch(__instance.gameObject);
			if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds < 1000)
			{
				return;
			}
			stopwatch.Restart();
			float value = 0f;
			bool flag = false;
			bool flag2 = false;
			if (__instance.m_name.Equals(FirePlaceDefinitions.FirePitName))
			{
				if (!FireplaceConfiguration.FireplaceIsEnabled.Value || !FireplaceConfiguration.FireplaceAutoFuel.Value)
				{
					return;
				}
				flag2 = true;
				value = FireplaceConfiguration.FireplaceAutoRange.Value;
				flag = false; 
			}
			else if (__instance.m_name.Equals(FirePlaceDefinitions.HearthName))
			{
				if (!FireplaceConfiguration.FireplaceIsEnabled.Value || !FireplaceConfiguration.FireplaceAutoFuel.Value)
				{
					return;
				}
				flag2 = true;
				value = FireplaceConfiguration.FireplaceAutoRange.Value;
				flag = false;
			}
			else if (__instance.m_name.Equals(FirePlaceDefinitions.BonfireName))
			{
				if (!FireplaceConfiguration.FireplaceIsEnabled.Value || !FireplaceConfiguration.FireplaceAutoFuel.Value)
				{
					return;
				}
				flag2 = true;
				value = FireplaceConfiguration.FireplaceAutoRange.Value;
				flag = false;
			}
			else if (__instance.m_name.Contains("torch"))
			{
				if (!FireplaceConfiguration.FireplaceIsEnabled.Value || !FireplaceConfiguration.TorchAutoFuel.Value)
				{
					return;
				}
				flag2 = true;
				value = FireplaceConfiguration.TorchAutoRange.Value;
				flag = false;
			}
			else if (__instance.m_name.Contains("brazier"))
			{
				if (!FireplaceConfiguration.FireplaceIsEnabled.Value || !FireplaceConfiguration.BrazierAutoFuel.Value)
				{
					return;
				}
				flag2 = true;
				value = FireplaceConfiguration.BrazierAutoRange.Value;
				flag = false;
			}
			value = helper.Clamp(value, 1f, 50f);
			//int num = __instance.m_maxOre - __instance.GetQueueSize();
			int num2 = (int)__instance.m_maxFuel - (int)Math.Ceiling(__instance.m_nview.GetZDO().GetFloat(ZDOVars.s_fuel));
			if ((bool)__instance.m_fuelItem && num2 > 0)
			{
				ItemDrop.ItemData itemData = __instance.m_fuelItem.m_itemData;
				int num3 = InventoryAssistant.RemoveFireWoodItemInAmountFromAllNearbyChests(__instance.gameObject, value, itemData, num2, !flag);
				for (int i = 0; i < num3; i++)
				{
					__instance.m_nview.InvokeRPC("RPC_AddFuel");
				}
				if (num3 > 0 && Configuration.enableDebugLogging.Value)
				{
					ZLog.Log("Added " + num3 + " fuel(" + itemData.m_shared.m_name + ") in " + __instance.m_name);
				}
			}
		

		}
	}

}

