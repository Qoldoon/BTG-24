using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private float boostSpeed = 2f;
    private float speedUpDuration = 5f;
    private PlayerController controller;

    private void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(Vector2.Distance(transform.position, player.transform.position) > 3) return;
        var vector = player.transform.position - transform.position;
        
        transform.position += vector.normalized / (vector.magnitude * 100f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            controller = collision.GetComponent<PlayerController>();
            controller.speedMult = boostSpeed;
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
