using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallCache
{
    public static List<Vector3> WallPositions { get; private set; }
    public static List<GameObject> WallObjects { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("WallManager initializing...");

        WallPositions = new List<Vector3>();
        WallObjects = new List<GameObject>();

        // Assuming walls are tagged "Wall"
        var walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var wall in walls)
        {
            WallObjects.Add(wall);
            WallPositions.Add(wall.transform.position);
        }

        Debug.Log($"WallManager: found {WallObjects.Count} walls.");
    }
}