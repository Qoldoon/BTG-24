using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalHit : MonoBehaviour
{
    
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        if (collision.tag == "Walls") { Destroy(gameObject); return; }

        if (collision.GetComponent<EnemyHealth>() == null) return;
        
        collision.GetComponent<EnemyHealth>().Hit(damage);
      

        Destroy(gameObject);
            
    }
}
