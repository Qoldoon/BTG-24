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
            if(collision.GetComponent<PlayerInventory>().hasKey("exit"))
            {
                var controller = collision.GetComponent<PlayerController>();
                Destroy(GameObject.Find("SelectedItems"));
                controller.speed = 5f;
                ScoreManager.instance.SetHighscore();
                MainMenu.CompleteLevel(level);
                //Score sheet
                StartCoroutine(LevelEnding(collision));
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
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelect");
    }
}
