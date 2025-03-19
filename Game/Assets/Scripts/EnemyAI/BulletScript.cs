using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    public TrailRenderer bulletTrail;
    [SerializeField] public float speed = 30f;
    public int damage = 50;
    private Vector2 previousPosition;
    private Vector2 newPosition;
    public Vector2 direction;
    public bool forEnemy = false;
    public int mainmenu;
    private LayerMask collisionMask;

    void Start()
    {
        previousPosition = transform.position;
        
        
        if (bulletTrail == null && TryGetComponent(out TrailRenderer trail))
        {
            bulletTrail = trail;
        }
        
        collisionMask = Physics2D.AllLayers & ~(1 << gameObject.layer);
        
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
        if (forEnemy) return;
        
        forEnemy = true;
        direction = -direction;
        newPosition = newPosition + direction * Vector2.Distance(hit.point, newPosition);
        
        if (bulletTrail != null)
        {
            bulletTrail.Clear();
            bulletTrail.startColor = Color.blue;
            bulletTrail.endColor = Color.cyan;
        }
    }
    void HandleCollision(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Player") && !forEnemy)
        {
            PlayerController playerController = hit.collider.GetComponent<PlayerController>();
            if (playerController.IsParrying())
            {
                Parry(hit);
                return;
            }

            playerController.Hit();
            
            Destroy(gameObject);
        }
        else if (hit.collider.CompareTag("Enemy") && forEnemy)
        {
            hit.collider.GetComponent<EnemyHealth>().Hit(damage);
            Destroy(gameObject);
        }
        else if (hit.collider.CompareTag("Walls"))
        {
            Destroy(gameObject);
            return;
        }
        else if (hit.collider.CompareTag("Glass"))
        {
            Destroy(hit.collider.gameObject);
        }
    }
}