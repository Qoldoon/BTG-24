using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private const string HIGHEST_LEVEL_KEY = "HighestLevelUnlocked";
    private static string nextScene;
    public void PlayLevel(int level)
    { 
        Debug.Log("Playing Game");
        SetNext(level);
        if (IsLevelUnlocked(level))
            SceneManager.LoadScene("PreGameMenu");
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void SetNext(int level)
    {
        nextScene = "Level_" + level;
    }
    public void LoadNext()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void Delete(GameObject obj)
    {
        Destroy(obj);
    }
    public void QuitGame()
    {
        Debug.Log("Quit works:)");
        Application.Quit();
    }
    
    public static void CompleteLevel(int currentLevel)
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
        
        if (currentLevel + 1 > highestLevelUnlocked)
        {
            PlayerPrefs.SetInt(HIGHEST_LEVEL_KEY, currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
    
    private static bool IsLevelUnlocked(int level)
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 1);
        return level <= highestLevelUnlocked;
    }
}
