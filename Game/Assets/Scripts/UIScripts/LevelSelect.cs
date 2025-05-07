using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public int level;
    MainMenu mainMenu;
    public Text name;
    public Text score;

    private void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenu>();
        name.text = $"Level {level}";
        score.text = $"Highscore: {ScoreManager.ShowHighscore(level)}";
    }

    public void Load()
    {
        mainMenu.PlayLevel(level);
    }
}
