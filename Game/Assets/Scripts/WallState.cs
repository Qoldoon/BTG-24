using UnityEngine;

public class WallState : MonoBehaviour,  IDamagable
{
    public Sprite damagedsprite;
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false)
    {
        HitResponseBuilder hb = new HitResponseBuilder().Damage(damage).Target(target).Destroy();
        if(damage <= 50f) return hb.Build();
        spriteRenderer.sprite = damagedsprite;
        GetComponent<Collider2D>().enabled = false;
        AstarPath.active.Scan();
        return hb.Build();
    }
}
