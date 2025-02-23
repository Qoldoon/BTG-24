using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    GameObject target;
    public TrailRenderer bulletTrail;
    public float speed = 30f;
    private bool hasBeenParried = false;
    Rigidbody2D bulletRB;
    private Vector2 direction;
    public int mainmenu;
    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        
        if (bulletTrail == null && TryGetComponent<TrailRenderer>(out TrailRenderer trail))
        {
            bulletTrail = trail;
        }
        
        direction = (target.transform.position - transform.position).normalized * speed;
        bulletRB.linearVelocity = new Vector2(direction.x, direction.y);
        Destroy(this.gameObject, 5f);
    }
    public void Parry()
    {
        if (hasBeenParried) return;
        
        hasBeenParried = true;
        bulletRB.linearVelocity = -direction * (speed * 2f);
        
        if (bulletTrail != null)
        {
            bulletTrail.startColor = Color.blue;
            bulletTrail.endColor = Color.cyan;
        }
        
        gameObject.layer = LayerMask.NameToLayer("ParriedProjectile");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsParrying())
            {
                Parry();
                return;
            }
            //playerController.hitPoints--;

            if (playerController.hitPoints < 1)
            {
                playerController.Die(mainmenu);
            }
            Destroy(gameObject);
        }
        if (collision.tag == "Walls")
        {
            Destroy(gameObject);
            return;
        }
        if (collision.tag == "Glass")
        {
            Destroy(collision.gameObject);
        }
    }
}
