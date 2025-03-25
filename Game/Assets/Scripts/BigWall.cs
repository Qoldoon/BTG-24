using System;
using UnityEngine;

public class BigWall : MonoBehaviour, IDamagable
{
    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false)
    {
        WallState hitBlock = FindClosestBlock(hit);

        if (hitBlock != null)
        {
            return hitBlock.Hit(hit, damage, target, emp);
        }

        throw new InvalidOperationException("No hit");
    }
    WallState FindClosestBlock(Vector2 hitPoint)
    {
        WallState closestBlock = null;
        float minDistance = float.MaxValue;
        
        foreach (WallState block in gameObject.GetComponentsInChildren<WallState>())
        {
            float distance = Vector2.Distance(block.transform.position, hitPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBlock = block;
            }
        }

        return closestBlock;
    }

}
