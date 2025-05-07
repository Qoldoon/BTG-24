using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrbPickup : MonoBehaviour
{
    public float changeInterval = 3f;
    private float timer;
    SpriteRenderer spriteRenderer;
    Color color;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update()
    {
        float brightness = Mathf.Lerp(0.8f, 1.1f, Mathf.PingPong(Time.time * changeInterval, 1f));
        spriteRenderer.color = color.MultiplyRGB(brightness);
    }
}
