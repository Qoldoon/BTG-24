using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Behaviour : MonoBehaviour
{
    [Header("Nodes")]
    public Transform aimTarget;
    public Transform movementTarget;
    [Header("Vision")]
    public float visionRange = 12f;
    public float visionAngle = 200f;
    [Header("Combat")]
    public GameObject bullet;
    public float fireRate = 1f;
    public float nextFireTime;
    
    private AIDestinationSetter setter;
    private Vector3 memory;
    private bool isAggro;
    private Coroutine aggroCoroutine;

    
    void Awake()
    {
        if (aimTarget == null || movementTarget == null)
        {
            Debug.LogError("target not assigned!");
            enabled = false;
            return;
        }
        
        setter = GetComponent<AIDestinationSetter>();
        if (setter == null)
        {
            Debug.LogError("setter not found!");
            enabled = false;
        }
    }
    void Start()
    {
        setter.target = movementTarget;
        movementTarget.transform.parent = null;
        aimTarget.transform.parent = null;
    }
    
    void Update()
    {
        Rotate();
    }

    private void Shoot()
    {
        if (nextFireTime >= Time.time) return;
        var bullet = Instantiate(this.bullet, transform.position + transform.up * 0.6f, Quaternion.identity);
        var projectile = bullet.GetComponent<Projectile>();
        projectile.speed = 30;
        projectile.direction = (aimTarget.transform.position - transform.position).normalized;
        projectile.damage = 50;
            
        nextFireTime = Time.time + fireRate;
    }
    private void Rotate()
    {
        if (aimTarget == null) return;
        Vector2 direction = (aimTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        Quaternion startRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, 1f);
    }
    
    public void Detection(Collider2D other)
    {
        Vector3 directionToTarget = (other.transform.position - transform.position).normalized;
        float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

        if (angleToTarget <= visionAngle / 2)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;
            var hit = Physics2D.Raycast(transform.position + directionToTarget * 0.51f, directionToTarget, visionRange);
            if (hit.collider != null)
                HandleDetection(hit);
        }
    }

    private void HandleDetection(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            SetAggro(true);   
            memory= hit.collider.attachedRigidbody.linearVelocity;
            aimTarget.position = hit.point;
            if (Vector3.Distance(transform.position, hit.collider.transform.position) >= 8.4f)
                KeepDistance(hit.point);
            else StandStill();
            Shoot();
        }
        else if (hit.collider.CompareTag("Enemy"))
        {
            var b = hit.collider.GetComponent<Behaviour>();
            if (b.isAggro)
            {
                movementTarget.position = b.aimTarget.position;
                aimTarget.position = b.aimTarget.position;
            }
        }
        else
        {
            StartLosingAggro();
            if (memory != Vector3.zero)
            {
                ToLastSeenPos();
                aimTarget.position += memory * 0.5f;
                memory = Vector3.zero;
            }
        }

    }

    private void SetAggro(bool state)
    {
        isAggro = state;
        
        if (aggroCoroutine != null)
        {
            StopCoroutine(aggroCoroutine);
            aggroCoroutine = null;
        }
    }
    
    private void StartLosingAggro()
    {
        if (aggroCoroutine == null)
        {
            aggroCoroutine = StartCoroutine(LoseAggroAfterDelay());
        }
    }
    
    private IEnumerator LoseAggroAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        isAggro = false;
        aggroCoroutine = null;
    }
    private void KeepDistance(Vector3 point)
    {
        Vector3 directionToTarget = (point - transform.position).normalized;
        movementTarget.position = point - directionToTarget * 8f;
    }
    private void ToLastSeenPos()
    {
        movementTarget.position = aimTarget.position;
    }
    private void StandStill()
    {
        movementTarget.position = transform.position;
    }

    private void OnDestroy()
    {
        if (movementTarget != null)
            Destroy(movementTarget.gameObject);
            
        if (aimTarget != null)
            Destroy(aimTarget.gameObject);
    }
}
