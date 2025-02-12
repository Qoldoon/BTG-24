using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float speed = PlayerStats.GetSpeed();
    
    public Rigidbody2D rb;
    float t;
    private void Start()
    {
        t = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        moveHandler();
        dodgeHandler();
        speed = PlayerStats.GetSpeed();
    }

    void dodgeHandler()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (Time.time < t) return;
        StartCoroutine(Dodge());
        t = Time.time + 1f;
    }
    void moveHandler()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveDir = Vector2.ClampMagnitude(new Vector2(x, y), 1);
        rb.linearVelocity = moveDir * speed;
    }
    IEnumerator Dodge()
    {
        var orgSpeed = speed;
        PlayerStats.SetSpeed(25f);
        yield return new WaitForSeconds(0.15f);
        PlayerStats.SetSpeed(orgSpeed);
    }
}
