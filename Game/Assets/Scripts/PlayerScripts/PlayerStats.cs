using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public static int hitPoints = 1;
    [SerializeField] public static float speed = 5f;


    public static int GetHitPoints()
    {
        return hitPoints;
    }
    public static float GetSpeed()
    {
        return speed;
    }
    public static void SetSpeed(float speedToSet)
    {
        speed = speedToSet;
    }
    public static void AddHitPoint()
    {
        hitPoints++;
    }

    public static void SubtractHitPoint()
    {
        hitPoints--;
    }
    public static void Die(int scene)
    {
        Destroy(GameObject.Find("SelectedItems"));
        speed = 5f;
        SceneManager.LoadScene(scene);
    }
   
}
