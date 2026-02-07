using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

internal static class GameObjectAssistant
{
    private static Dictionary<int, Stopwatch> stopwatches = new Dictionary<int, Stopwatch>();

    public static Stopwatch GetStopwatch(GameObject o)
    {
        int instanceId = o.GetInstanceID();
        
        if (!stopwatches.TryGetValue(instanceId, out Stopwatch value))
        {
            value = new Stopwatch();
            stopwatches[instanceId] = value;
        }
        
        return value;
    }

    public static T GetChildComponentByName<T>(string name, GameObject objected) where T : Component
    {
        T[] componentsInChildren = objected.GetComponentsInChildren<T>(includeInactive: true);
        foreach (T val in componentsInChildren)
        {
            if (val.gameObject.name == name)
            {
                return val;
            }
        }
        return null;
    }
}
