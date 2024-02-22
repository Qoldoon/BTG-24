using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private float boostSpeed = 10f;
    private float originalSpeed = PlayerStats.GetSpeed();
    private float speedUpDuration = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            PlayerStats.SetSpeed(boostSpeed);
            StartCoroutine(SpeedUpDuration());
           
        }
    }
    IEnumerator SpeedUpDuration()
    {
        yield return new WaitForSeconds(speedUpDuration);
        PlayerStats.SetSpeed(originalSpeed);
        Destroy(gameObject);
    }
}
