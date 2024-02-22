using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public static string nextScene;
    public void PlayGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void SetNext(string scene)
    {
        nextScene = scene;
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
    public static void setImage(string path)
    {
        ImageLoad.image = Application.dataPath + path;
        Debug.Log(ImageLoad.image);
    }
}
