using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public int mainmenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponent<PlayerController>();
            controller.hitPoints--;
            if (controller.hitPoints < 1)
            {
                /*ScoreManager.instance.FinalHighScore();*/
                controller.Die(mainmenu);
            }
        }
    }
   
}
