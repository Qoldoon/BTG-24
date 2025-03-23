using UnityEngine;

public class WallState : MonoBehaviour,  IDamagable
{
    public Sprite damagedsprite;
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public bool Hit(float damage, int target, bool emp = false)
    {
        if(damage <= 50f) return true;
        spriteRenderer.sprite = damagedsprite;
        GetComponent<Collider2D>().enabled = false;
        return true;
    }
}
