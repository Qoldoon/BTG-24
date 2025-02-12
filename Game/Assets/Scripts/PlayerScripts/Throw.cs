using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public int grenadeCount = 6;
    public float fireRate = 1;
    public float grenadeSpeed = 20;
    public float reloadTime = 1;
    public float throwDistance = 5;
    public GameObject grenade;
    float time;
    private void Start()
    {
        time = Time.time;
    }
    public void Attack()
    {
        if (grenadeCount == 0) return;
        if (Time.time < time) return;

        grenade.GetComponentInChildren<GrenadeTraveling>().explosionDistance = throwDistance;

        Vector3 euler = transform.eulerAngles;
        var o = euler;
        transform.eulerAngles = euler;

        GameObject projectile = Instantiate(grenade, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = transform.rotation * Vector2.up * grenadeSpeed;

        grenadeCount--;
        time = Time.time + fireRate;
        transform.eulerAngles = o;
    }
}
