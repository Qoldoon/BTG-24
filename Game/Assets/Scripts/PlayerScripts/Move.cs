using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    public Rigidbody2D rb;
    float t;
    private void Start()
    {
        t = Time.time;
    }
}
