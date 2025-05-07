using System;
using UnityEngine;

public class BulletScript : Projectile
{
    public TrailRenderer bulletTrail;
    private Vector2 previousPosition;
    private Vector2 newPosition;
    private LayerMask collisionMask;

    void Start()
    {
        previousPosition = transform.position;
        
        
        if (bulletTrail == null && TryGetComponent(out TrailRenderer trail))
        {
            bulletTrail = trail;
        }
        
        collisionMask = Physics2D.AllLayers & ~(1 << gameObject.layer);

        if (emp && bulletTrail != null)
        {
            bulletTrail.Clear();
            bulletTrail.startColor = Color.blue;
            bulletTrail.endColor = Color.cyan;
        }
        
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        newPosition = (Vector2)transform.position + direction * (speed * Time.deltaTime);
            
        float movementDistance = Vector2.Distance(previousPosition, newPosition);
        
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, direction, movementDistance, collisionMask);
            
        if (hit.collider != null)
        {
            HandleCollision(hit);
        }
        
        if (this != null)
        {
            transform.position = newPosition;
        }

        previousPosition = transform.position;
    }
    public void Parry(RaycastHit2D hit)
    {
        direction = -direction;
        newPosition += direction * Vector2.Distance(hit.point, newPosition);
        
        if (bulletTrail != null)
        {
            bulletTrail.Clear();
            bulletTrail.startColor = Color.blue;
            bulletTrail.endColor = Color.cyan;
        }
    }
    void HandleCollision(RaycastHit2D hit)
    {
        var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            HitResponse response = damageable.Hit(damage, target, emp);
            damage = response.damage;
            target = response.target;
            if (response.reflect)
            {
                Parry(hit);
            }
            if(response.destroy)
                EndPath();
        }
        else if(!hit.collider.isTrigger)
        {
            EndPath();
        }
    }

    void EndPath()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        speed = 0f;
        bulletTrail.time = 0.05f;
        Destroy(gameObject, 0.05f);
        enabled = false;
    }
}