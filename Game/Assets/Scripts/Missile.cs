using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public Rigidbody2D rb;
    private bool emp = false;
    public GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
    }
    
    public void Parry()
    {
        direction = -direction;
        rb.linearVelocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            HitResponse response = damageable.Hit(transform.position, damage, target, emp, 2);
            damage = response.damage;
            target = response.target;
            if (response.reflect)
            {
                Parry();
            }
            if(response.destroy)
                Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        if (explosion != null)
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 2);

        foreach (Collider2D hit in hitObjects)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Hit(transform.position, 100, target, emp, 2);
            }
        }
    }
}
