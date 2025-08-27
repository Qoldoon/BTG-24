using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public int level;
    private int score;
    private int highscore;

    private void Awake()
    {
        instance = this;
    }
    
    public void AddPoints(int amount)
    {
        score += amount;
    }

    public string ShowScore()
    {
        return "Score: " + score;
    }

    public static string ShowHighscore(int l)
    {
        var data = SaveSystem.LoadGame();
        if (data == null || data.highscores == null || l < 0 || l >= data.highscores.Count)
            return "0";

        return data.highscores[l].ToString();
    }

    public void SetHighscore(int l)
    {
        var save = SaveSystem.LoadGame();
        
        while (save.highscores.Count <= l)
            save.highscores.Add(0);
        
        int current = save.highscores[l];
        if (score > current)
            save.highscores[l] = score;
        
        SaveSystem.SaveGame(save);
    }
}
