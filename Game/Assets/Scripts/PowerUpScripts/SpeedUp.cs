using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private float boostSpeed = 2f;
    private float speedUpDuration = 5f;
    private PlayerController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            controller = collision.GetComponent<PlayerController>();
            controller.speedMult = boostSpeed;
            controller.playerInventory.canvas.CreateText("Speed");
            StartCoroutine(SpeedUpDuration());
        }
    }
    IEnumerator SpeedUpDuration()
    {
        yield return new WaitForSeconds(speedUpDuration);
        controller.speedMult = 1f;
        Destroy(gameObject);
    }
}
