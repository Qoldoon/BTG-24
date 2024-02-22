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
            PlayerStats.SubtractHitPoint();
            if (PlayerStats.GetHitPoints() < 1)
            {
                /*ScoreManager.instance.FinalHighScore();*/
                PlayerStats.Die(mainmenu);

            }
        }
    }
   
}
