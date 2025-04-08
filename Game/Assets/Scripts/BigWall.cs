using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BigWall : MonoBehaviour, IDamageable
{
    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false, float radius = 0)
    {
        List<WallState> hitBlocks = FindClosestBlocks(hit, radius);
        HitResponse finalResponse = null;
        if (hitBlocks.Count == 0)
        {
            return new HitResponseBuilder().Build();
        }
        if (radius == 0)
        {
            finalResponse = hitBlocks[0].Hit(hit, damage, target, emp);
            AstarPath.active?.Scan();
            return finalResponse;
        }
        
        
        foreach (var block in hitBlocks)
        {
            finalResponse = block.Hit(hit, damage, target, emp);
        }
        if (hitBlocks.Count > 0)
        {
            AstarPath.active?.Scan();
        }
        
        return finalResponse ?? new HitResponseBuilder().Destroy().Build();
    }
    private List<WallState> FindClosestBlocks(Vector2 hitPoint, float radius)
    {
        var closestBlocks = new List<WallState>();
        WallState closestSingleBlock = null;
        float minDistance = float.MaxValue;
        WallState[] wallBlocks = GetComponentsInChildren<WallState>();
        
        foreach (WallState block in wallBlocks)
        {
            float distance = Vector2.Distance(block.transform.position, hitPoint);
            if (radius > 0 && distance <= radius)
            {
                closestBlocks.Add(block);
            }
            if (radius == 0 && distance < minDistance)
            {
                minDistance = distance;
                closestSingleBlock = block;
            }
        }
        if (radius == 0 && closestSingleBlock != null)
        {
            closestBlocks.Add(closestSingleBlock);
        }
        return closestBlocks;
    }

}
