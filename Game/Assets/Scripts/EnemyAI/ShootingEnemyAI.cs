using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShootingEnemyAI : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject Shooter;
    public float ShootingRange;
    public Transform target;
    private float fireRate = 1f;
    private float nextFireTime;
    public bool followEnabled = true;
    public float activateDistance = 20f;
    private Path path;
    bool temp = false;
    void Update()
    {
       float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
        if (TargetInDistance() && temp == false)
        {
            GetComponent<AIDestinationSetter>().enabled = true;
        }
        if (distanceFromPlayer < ShootingRange && nextFireTime < Time.time)
        {
            Instantiate(Bullet, transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            GetComponent<AIDestinationSetter>().enabled = false;
            temp = true;
        }
        else if (distanceFromPlayer > ShootingRange)
        {
            temp = false;
        }
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ShootingRange);
    }
}
