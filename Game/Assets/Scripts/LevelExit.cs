using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.GetComponent<PlayerInventory>().hasKey)
            {
                var controller = collision.GetComponent<PlayerController>();
                Destroy(GameObject.Find("SelectedItems"));
                controller.speed = 5f;
                //ScoreManager.instance.FinalHighScore();
                MainMenu.CompleteLevel(level);
                SceneManager.LoadScene("MenuScene");
            }
        }
        
    }
    
}
