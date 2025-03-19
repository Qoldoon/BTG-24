using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    Color color;
    float time;
    public int damage;
    public bool emp;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = gameObject.GetComponent<SpriteRenderer>().color;
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > time)
        {
            time += 0.01f;
            color.a -= 0.01f;
            if (color.a > 0)
                _spriteRenderer.color = color;
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
