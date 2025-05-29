using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int level;

    private void Start()
    {
        ScoreManager.instance.level = level;
        ScoreManager.instance.AddPoints(100 * level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.GetComponent<PlayerInventory>().HasKey("exit"))
            {
                StartCoroutine(LevelEnding(collision));
                Destroy(GameObject.Find("SelectedItems"));
                ScoreManager.instance.SetHighscore();
                MainMenu.CompleteLevel(level);
            }
            else
            {
                collision.GetComponent<PlayerInventory>().canvas.CreateText("Missing key");
            }
        }
        
    }

    IEnumerator LevelEnding(Collider2D collision)
    {
        var inv = collision.GetComponent<PlayerInventory>();
        inv.playerUI.TitleText("COMPLETE");
        inv.playerUI.SubTitleText(ScoreManager.instance.ShowScore());
        inv.GetComponent<PlayerController>().hitPoints = 9999;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelect");
    }
}
