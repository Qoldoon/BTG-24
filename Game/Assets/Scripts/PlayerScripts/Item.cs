using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ammoCount = 6;
    public float fireRate = 1;
    public float bulletSpeed = 30;
    public int bulletDamage = 50;
    public float bulletSpread = 0;
    public float reloadTime = 1;
    public GameObject Bullet;
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

        var bullet = Instantiate(Bullet, transform.position + transform.up * 0.3f, Quaternion.identity);
        if (bullet.TryGetComponent(out Projectile projectile))
        {
            projectile.direction = (transform.rotation * Vector2.up).normalized;
            projectile.speed = bulletSpeed;
            projectile.damage = bulletDamage;
            projectile.target = 1;
        }
        
        ammoCount--;
        time = Time.time + fireRate;
        transform.eulerAngles = o;
    }
}