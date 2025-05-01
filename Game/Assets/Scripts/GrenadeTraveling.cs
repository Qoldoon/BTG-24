using UnityEngine;
using UnityEngine.SceneManagement;

public class GrenadeTraveling : Projectile
{
    public Rigidbody2D rb;
    private bool emp = false;
    public float detonationTime = 1f;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
        rb.angularVelocity = 300;
        
        Destroy(gameObject, detonationTime);
    }
    void OnDestroy()
    {
        if (explosion != null)
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(effect, 1f); 
        }
        SoundTracker.EmitSound(gameObject);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 2);
        
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damagable))
            {
                damagable.Hit(damage, target, emp);
            }
        }
    }
}