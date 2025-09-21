using HarmonyLib;
using Jotunn.Managers;
using TillValhalla.Configurations;
using TillValhalla.Configurations.Sections;
using UnityEngine;

namespace TillValhalla.GameClasses
{
    [HarmonyPatch(typeof(Ship), "Awake")]
    public static class Ship_Awake_Patch
    {
        public static void Postfix(Ship __instance)
        {
            __instance.m_stearForce = ShipConfiguration.stearforce.Value;
            __instance.m_rudderSpeed = ShipConfiguration.rudderspeed.Value;
            __instance.m_sailForceFactor = ShipConfiguration.sailforcefactor.Value;
            if (ShipConfiguration.shipFireRested.Value)
            {
                AddFireWarmthToShip(__instance);
            }
                


        }
        private static void AddFireWarmthToShip(Ship ship)
        {
            // Get fire_pit prefab
            GameObject firePitPrefab = PrefabManager.Instance.GetPrefab("fire_pit");
            if (!firePitPrefab)
            {
                Jotunn.Logger.LogError("fire_pit prefab not found!");
                return;
            }

            // Find _enabled_high > firewarmth hierarchy
            Transform enabledHighTransform = firePitPrefab.transform.Find("_enabled_high");
            if (!enabledHighTransform)
            {
                Jotunn.Logger.LogError("_enabled_high child not found in fire_pit!");
                return;
            }

            Transform firewarmthTransform = enabledHighTransform.Find("FireWarmth");
            if (!firewarmthTransform)
            {
                Jotunn.Logger.LogError("firewarmth child not found!");
                return;
            }

            // Clone firewarmth GameObject
            GameObject firewarmthClone = Object.Instantiate(firewarmthTransform.gameObject, ship.transform);
            firewarmthClone.name = "firewarmth";

            // Create _enabled_high parent for hierarchy
            GameObject enabledHighClone = new GameObject("_enabled_high");
            enabledHighClone.transform.SetParent(ship.transform);
            firewarmthClone.transform.SetParent(enabledHighClone.transform);
            firewarmthClone.GetComponent<SphereCollider>().radius = 12;
        }

    }
}