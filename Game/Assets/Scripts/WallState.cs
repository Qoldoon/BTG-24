using UnityEngine;

public class WallState : MonoBehaviour,  IDamageable
{
    public Sprite damagedsprite;
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public HitResponse Hit(Vector2 hit, float damage, int target, bool emp = false, float radius = 0)
    {
        HitResponseBuilder hb = new HitResponseBuilder();
        if(damage <= 50f) return hb.Destroy().Build();
        spriteRenderer.sprite = damagedsprite;
        GetComponent<Collider2D>().enabled = false;
        hb.Damage(damage - 10);
        return hb.Build();
    }
}
