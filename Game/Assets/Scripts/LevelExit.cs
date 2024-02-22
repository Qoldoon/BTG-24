using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.GetComponent<PlayerInventory>().hasKey)
            {
                Destroy(GameObject.Find("SelectedItems"));
                PlayerStats.SetSpeed(5f);
               /* ScoreManager.instance.FinalHighScore();*/
                SceneManager.LoadScene("MenuScene");
            }
        }
        
    }
    
}
