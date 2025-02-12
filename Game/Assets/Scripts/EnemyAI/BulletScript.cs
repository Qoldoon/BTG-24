using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    public int mainmenu;
    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.linearVelocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.SubtractHitPoint();

            if (PlayerStats.GetHitPoints() < 1)
            {
                PlayerStats.Die(mainmenu);

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
