using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    public int playerHighscore;
    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore");
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = " HIGHSCORE: " + highscore.ToString();
        
    }

    // Update is called once per frame
    public void AddPointForMeleeKill(string enemyTag)
    {
        // ArmoredEnemy, KeyEnemy,LightEnemy,NormalEnemy,ShieldedEnemy
        switch (enemyTag) 
        {
            case "ArmoredEnemy":
                score += 300;
                break;
            case "KeyEnemy":
                score += 500;
                break;
            case "LightEnemy":
                score += 100;
                break;
            case "NormalEnemy":
                score += 50;
                break;
            case "ShieldedEnemy":
                score += 250;
                break;
        }
        scoreText.text = score.ToString() + " POINTS";
        if (highscore < score)
        PlayerPrefs.SetInt("highscore", score);
    }
   /* public void FinalHighScore()
    {
        PlayerPrefs.SetInt("highscore2", score/((int)Mathf.Round(Time.timeSinceLevelLoad)));
    }*/
}
