using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float explosionDistance = 5;
    public GameObject explosion;
    public float radius = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        if (collision.tag == "Walls" || collision.tag == "Glass")
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
