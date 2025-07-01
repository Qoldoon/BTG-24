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
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            controller = collision.GetComponent<PlayerController>();
            controller.MultiplySpeed(boostSpeed, speedUpDuration);
            controller.playerInventory.canvas.CreateText("Speed");
            Destroy(gameObject);
        }
    }
}
