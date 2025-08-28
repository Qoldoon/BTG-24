using UnityEngine;
using UnityEngine.Tilemaps;

public class WallState : MonoBehaviour,  IDamageable
{
    private Tilemap tilemap;
    private Vector3Int cellPosition;
    
    void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
        cellPosition = tilemap.WorldToCell(transform.position);
    }

    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().ForAll();
        if(damage <= 120f) return hb.Destroy().Build();
        DestroyWall();
        return hb.Build();
    }

    private void DestroyWall()
    {
        GetComponent<Collider2D>().enabled = false;
        AstarPath.active?.Scan();
        tilemap.SetTile(cellPosition, null);
    }
}
