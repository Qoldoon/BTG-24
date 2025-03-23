using UnityEngine;
using UnityEngine.SceneManagement;

public class GrenadeTraveling : MonoBehaviour
{
    public TrailRenderer bulletTrail;
    [SerializeField] public float speed = 30f;
    public int damage = 50;
    public Rigidbody2D rb;
    public Vector2 direction;
    private bool emp = false;
    public bool forEnemy = false;
    public float detonationTime = 1f;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
        rb.angularVelocity = 300;
        
        Destroy(gameObject, detonationTime);
    }
    void Update()
    {
    }
    void OnDestroy()
    {
        if (explosion != null)
        {
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(effect, 1f); 
        }
        
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 4);
        
        foreach (Collider2D hit in hitObjects)
        {
            hit.gameObject.GetComponent<IDamagable>().Hit(damage, 3, emp);
            // if (hit.GetComponent<EnemyHealth>() != null)
            //     hit.GetComponent<EnemyHealth>().Hit(damage, emp);
            // if (hit.tag == "Glass" && !emp)
            // {
            //     Destroy(hit.gameObject);
            //     AstarPath.active.Scan();
            // }  
        }
    }
}