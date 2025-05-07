using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrenadeTraveling : Projectile
{
    public Rigidbody2D rb;
    public float detonationTime = 1f;
    [SerializeField] private GameObject explosion;
    public int radius = 2;

    void Start()
    {
        emp = true;
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
        rb.angularVelocity = 300;
        
        StartCoroutine(timer(detonationTime));
    }

    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }
    void Explode()
    {
        if (explosion != null)
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            effect.transform.localScale *= radius * 2;
            Destroy(effect, 1f); 
        }
        SoundTracker.EmitSound(gameObject);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, radius);
        
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damagable))
            {
                damagable.Hit(damage, 2, emp);
            }
        }
        Destroy(gameObject);
    }
}