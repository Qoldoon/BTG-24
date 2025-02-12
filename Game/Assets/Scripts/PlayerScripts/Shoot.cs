using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public int ammoCount = 6;
    public float fireRate = 1;
    public float bulletSpeed = 100;
    public float bulletSpread = 0;
    public float reloadTime = 1;
    public GameObject bullet;
    float time;
    private void Start()
    {
        time = Time.time;
    }
    public void Attack()
    {
        if (ammoCount == 0) return;
        if (Time.time < time) return;

        Vector3 euler = transform.eulerAngles;
        var o = euler;
        euler.z = Random.Range(euler.z - bulletSpread, euler.z + bulletSpread);
        transform.eulerAngles = euler;

        GameObject projectile = Instantiate(bullet, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = transform.rotation * Vector2.up * bulletSpeed;

        ammoCount--;
        time = Time.time + fireRate;
        transform.eulerAngles = o;
    }
}
