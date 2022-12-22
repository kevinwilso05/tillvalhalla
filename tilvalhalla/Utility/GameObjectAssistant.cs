using System.Collections.Concurrent;
using System.Diagnostics;
using UnityEngine;

internal static class GameObjectAssistant
{
    private static ConcurrentDictionary<float, Stopwatch> stopwatches = new ConcurrentDictionary<float, Stopwatch>();

    public static Stopwatch GetStopwatch(GameObject o)
    {
        float gameObjectPosHash = GetGameObjectPosHash(o);
        Stopwatch value = null;
        if (!stopwatches.TryGetValue(gameObjectPosHash, out value))
        {
            value = new Stopwatch();
            stopwatches.TryAdd(gameObjectPosHash, value);
        }
        return value;
    }

    private static float GetGameObjectPosHash(GameObject o)
    {
        return 1000f * o.transform.position.x + o.transform.position.y + 0.001f * o.transform.position.z;
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
