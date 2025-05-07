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
        var s = (PlayerPrefs.GetInt("highscore") >> (l - 1) * 8) & 255;
        s *= 100;
        return s.ToString();
    }

    public void SetHighscore()
    {
        highscore = (PlayerPrefs.GetInt("highscore") >> (level - 1) * 8) & 255;
        highscore *= 100;
        if(highscore < score)
            highscore = score;
        highscore /= 100;
        highscore <<= (level - 1) * 8;
        var s = PlayerPrefs.GetInt("highscore");
        var mask = 15 << (level - 1) * 8;
        mask = ~mask;
        s = s & mask | highscore;
        PlayerPrefs.SetInt("highscore", s);
        PlayerPrefs.Save();
    }
}
