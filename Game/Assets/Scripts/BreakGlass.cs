using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player") return;

        if (collision.gameObject.tag == "Glass")
        {
            Debug.Log("Abra");
            Destroy(collision.gameObject);
            AstarPath.active.Scan();
        }
    }

  
}
