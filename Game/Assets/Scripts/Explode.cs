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
        Collider2D myCollider = gameObject.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        int colliderCount = myCollider.Overlap(contactFilter, colliders);
        Debug.Log("Collision count : " + colliderCount);
        for (int i = 0; i < colliderCount; i++)
        {
            if (colliders[i].GetComponent<EnemyHealth>() != null)
                colliders[i].GetComponent<EnemyHealth>().Hit(damage, emp);
            if (colliders[i].tag == "Glass" && !emp)
            {
                Destroy(colliders[i].gameObject);
                AstarPath.active.Scan();
            }   
        }
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
