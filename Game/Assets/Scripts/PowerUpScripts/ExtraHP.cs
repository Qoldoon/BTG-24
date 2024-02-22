using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraHP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.AddHitPoint();
            Destroy(gameObject);
        }
    }
}
