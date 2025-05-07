using UnityEngine;

public class WallState : MonoBehaviour,  IDamageable
{
    public Sprite damagedsprite;
    SpriteRenderer spriteRenderer;
    private BigWall parent;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parent = GetComponentInParent<BigWall>();
    }

    public HitResponse Hit(float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().ForAll();
        if(damage <= 120f) return hb.Destroy().Build();
        spriteRenderer.sprite = damagedsprite;
        GetComponent<Collider2D>().enabled = false;
        if(parent != null)
            parent.Scan();
        else AstarPath.active?.Scan();
        return hb.Build();
    }
}
