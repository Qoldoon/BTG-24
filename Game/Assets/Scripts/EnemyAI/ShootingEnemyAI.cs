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
    bool temp = false;
    private AIDestinationSetter setter;
    private AIPath path;
    void Start()
    {
        setter = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
    }
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
        if (TargetInDistance() && temp == false)
        {
            setter.enabled = true;
            path.enabled = true;
        }
        if (distanceFromPlayer < ShootingRange )
        {
            Rotate();
            
            setter.enabled = false;
            path.enabled = false;
            if (nextFireTime < Time.time)
            {
                Instantiate(Bullet, transform.position, Quaternion.identity);
                nextFireTime = Time.time + fireRate;
            }
            
            temp = true;
        }
        else if (distanceFromPlayer > ShootingRange)
        {
            temp = false;
        }
    }

    private void Rotate()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle-90);
        Quaternion startRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, 1f);
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