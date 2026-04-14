using UnityEngine;
using System.Collections.Generic;

public static class WallManager
{
    private static HashSet<Vector2Int> wallGridPositions = new ();
    
    
    public static void Initialize()
    {
        wallGridPositions.Clear();
        
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Walls");
        
        foreach (GameObject wall in walls)
        {
            Vector3 position = wall.transform.position;
            Vector2Int gridPosition = new Vector2Int(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y)
            );
            wallGridPositions.Add(gridPosition);
        }

        Debug.Log($"WallManager initialized with {wallGridPositions.Count} wall grid positions.");
    }
    
    public static bool IsWallAtGridPosition(Vector2Int gridPosition)
    {
        return wallGridPositions.Contains(gridPosition);
    }
    
    public static bool IsWallAtWorldPosition(Vector2 worldPosition)
    {
        Vector2Int gridPosition = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
        );
        return wallGridPositions.Contains(gridPosition);
    }
    
    public static void AddWallAtGridPosition(Vector2Int gridPosition)
    {
        wallGridPositions.Add(gridPosition);
    }
    
    public static void RemoveWallAtGridPosition(Vector2Int gridPosition)
    {
        wallGridPositions.Remove(gridPosition);
    }
}