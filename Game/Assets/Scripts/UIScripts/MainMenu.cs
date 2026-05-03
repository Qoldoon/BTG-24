using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private static string nextScene;
    public void PlayLevel(int level)
    {
        SetNext(level);
        if (IsLevelUnlocked(level))
            SceneManager.LoadScene("PreGameMenu");
    }

    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void SetNext(int level)
    {
        nextScene = string.IsNullOrEmpty(LevelSession.levelFile) ? "Level_" + level : "GameScene";
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
        var save = SaveSystem.LoadGame();
        
        if (currentLevel + 1 > save.highestLevelUnlocked)
        {
            save.highestLevelUnlocked = currentLevel + 1;
            SaveSystem.SaveGame(save);
        }
    }
    
    private static bool IsLevelUnlocked(int level)
    {
        int highestLevelUnlocked = SaveSystem.LoadGame().highestLevelUnlocked;
        return level <= highestLevelUnlocked;
    }
}
