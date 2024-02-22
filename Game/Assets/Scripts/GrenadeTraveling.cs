using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTraveling : MonoBehaviour
{
    public float explosionDistance = 5;
    public GameObject explosion;
    public float radius = 10;
    Vector3 startPosition;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distance = (transform.position - startPosition).magnitude;
        if (distance >= explosionDistance)
        {
            Explode();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        if (collision.tag == "Walls") 
        {
            Explode();
            Destroy(gameObject); 
            return; 
        }

        if (collision.GetComponent<EnemyHealth>() == null) return;
        Explode();
        Destroy(gameObject);

    }
    private void Explode()
    {
        GameObject projectile = Instantiate(explosion, transform.position, transform.rotation);
        projectile.transform.localScale = new Vector3(radius, radius, 0);
    }
}
