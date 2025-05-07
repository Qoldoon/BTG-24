using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public Rigidbody2D rb;
    public GameObject explosion;
    public TrailRenderer bulletTrail;
    public int radius = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        
        if (bulletTrail == null && TryGetComponent(out TrailRenderer trail))
        {
            bulletTrail = trail;
        }
        if (emp && bulletTrail != null)
        {
            bulletTrail.Clear();
            bulletTrail.startColor = Color.blue;
            bulletTrail.endColor = Color.cyan;
        }
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
            HitResponse response = damageable.Hit(damage, target, emp);
            damage = response.damage;
            target = response.target;
            if (response.reflect)
            {
                Parry();
            }
            if(response.destroy)
                Explode();
        }
        else if(!collision.isTrigger)
        {
            Explode();
        }
    }
    void Explode()
    {
        if (explosion != null)
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            effect.transform.localScale *= radius * 2;
            Destroy(effect, 1f);
        }
        
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, radius);
        
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Hit(100, target, emp);
            }
        }
        Destroy(gameObject);
    }
}
