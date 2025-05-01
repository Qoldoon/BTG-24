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

    private void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenu>();
        GetComponentInChildren<Text>().text = $"Level {level}";
    }

    public void Load()
    {
        mainMenu.PlayLevel(level);
    }
}
