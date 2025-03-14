using UnityEngine;
using UnityEngine.SceneManagement;

public class GrenadeTraveling : MonoBehaviour
{
    public TrailRenderer bulletTrail;
    [SerializeField] public float speed = 30f;
    public int damage = 50;
    public Rigidbody2D rb;
    public Vector2 direction;
    private bool emp = true;
    public bool forEnemy = false;
    public float detonationTime;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
        rb.angularVelocity = 300;
        if (bulletTrail == null && TryGetComponent(out TrailRenderer trail))
        {
            bulletTrail = trail;
        }
        
        detonationTime = Time.time + 1;
    }
    void Update()
    {
        if(Time.time >= detonationTime) Explode();
    }
    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}