using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAI;
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
    
    public IState currentState = new IdleState();
    
    private float nextFireTime;
    private AIDestinationSetter setter;
    public bool IsAggro => Sighting.IsRecent(_sightings.PlayerSighting(), 0.1f);
    private Sightings _sightings = new ();

    
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
        React();
        SetMoveTarget(movementTarget.position);
    }

    public void SetAimTarget(Vector3 targetPosition)
    {
        aimTarget.position = targetPosition;
    }

    public void SetMoveTarget(Vector3 targetPosition)
    {
        movementTarget.position = targetPosition;
        if(Vector2.Distance(transform.position, movementTarget.position) >= 1)
        {
            var dir = movementTarget.position - transform.position;
            var ray = Physics2D.Raycast(transform.position + dir.normalized * 0.51f, dir, 1f);
            if (ray.collider != null && ray.collider.CompareTag("Enemy"))
            {
                movementTarget.position = transform.position + RotateVector(dir, 30);
            }
        }
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(movementTarget.position, 1f);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            
            Vector2 direction = (movementTarget.position - hit.transform.position).normalized;
            if (direction == Vector2.zero)
                direction = Vector2.up;
            
            var overlapDistance = 1f - Vector2.Distance(movementTarget.position, hit.transform.position);
            if (overlapDistance > 0)
            {
                movementTarget.position += (Vector3)direction * overlapDistance;
            }
        }
    }
    
    private void Shoot()
    {
        if (nextFireTime >= Time.time) return;
        SoundTracker.EmitSound(gameObject);
        var o = Instantiate(bullet, transform.position + transform.up * 0.6f, Quaternion.identity);
        var projectile = o.GetComponent<Projectile>();
        projectile.speed = 30;
        projectile.direction = (aimTarget.position - transform.position).normalized;
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
        transform.rotation = Quaternion.RotateTowards(
            startRotation, 
            targetRotation, 
            360f * Time.deltaTime
        );
    }

    private void React()
    {
        currentState = currentState.ChangeState(_sightings);
        currentState.React(this);
    }
    
    public void Detection(Collider2D other)
    {
        Vector3 directionToTarget = (other.transform.position - transform.position).normalized;
        float angleToTarget = Vector2.Angle(transform.up, directionToTarget);
        //sight
        if (angleToTarget <= visionAngle / 2)
        {
            var hit = Physics2D.Raycast(transform.position + directionToTarget * 0.51f, directionToTarget, visionRange);
            if (hit.collider != null)
            {
                var sighting = new Sighting
                {
                    Target = hit.collider.gameObject,
                    Velocity = Vector3.ClampMagnitude(hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.linearVelocity : Vector3.zero, 5),
                    Position = hit.point,
                    TimeSeen = Time.time
                };
                _sightings.TryAddSighting(sighting);
            }
        }
        //hearing
        if (other.CompareTag("Player"))
        {
            var sound = SoundTracker.Listen();
            if (sound == null) return;
            var sighting = new Sighting
            {
                Target = sound,
                Velocity = Vector3.zero,
                Position = sound.transform.position,
                TimeSeen = Time.time
            };
            _sightings.TryAddSighting(sighting);
        }
    }

    private float time;
    public void LookAround()
    {
        if(time > Time.time) return;
        var dir = (aimTarget.position - transform.position).normalized;

        dir = RotateVector(dir, 180 + Random.Range(-60, 60));

        
        SetAimTarget(transform.position + dir * Random.Range(1.5f, 6));
        SetMoveTarget(transform.position + dir * 2);
        time = Time.time + Random.Range(1f, 1.4f);
    }

    public static Vector3 RotateVector(Vector2 direction, float angle)
    {
        float angleRadians = angle * Mathf.Deg2Rad;
        
        float cos = Mathf.Cos(angleRadians);
        float sin = Mathf.Sin(angleRadians);
        
        float x = direction.x * cos - direction.y * sin;
        float y = direction.x * sin + direction.y * cos;
    
        return new Vector2(x, y).normalized;
    }
    
    public void AttackPlayer(Sighting sighting)
    {
        SetAimTarget(sighting.Position);
        if (Vector3.Distance(transform.position, sighting.Target.transform.position) >= 8.4f)
            KeepDistance(sighting.Position);
        else StandStill();
        if(Vector2.Distance(transform.position, aimTarget.position) < 9f)
            Shoot();
    }
 
    public void Investigate(Sighting sighting)
    {
        SetAimTarget(sighting.Position);
        SetMoveTarget(sighting.Position);
    }

    public void FollowAlly(Sighting sighting)
    {
        var b = sighting.Target.GetComponent<Behaviour>();
        if (b.IsAggro)
        {
            SetMoveTarget(b.aimTarget.position);
            SetAimTarget(b.aimTarget.position);
        }
    }

    public void Chase(Sighting sighting)
    {
        SetMoveTarget(sighting.Position);
        SetAimTarget(movementTarget.position + sighting.Velocity * 0.5f);
    }

    public void ToPosition(Vector3 pos,  Vector3 dir)
    {
        SetMoveTarget(pos);
        SetAimTarget(pos + dir);
    }
    
    private void KeepDistance(Vector3 point)
    {
        Vector3 directionToTarget = (point - transform.position).normalized;
        SetMoveTarget(point - directionToTarget * 8f);
    }
    private void StandStill()
    {
        SetMoveTarget(transform.position);
    }

    private void OnDestroy()
    {
        if (movementTarget != null)
            Destroy(movementTarget.gameObject);
            
        if (aimTarget != null)
            Destroy(aimTarget.gameObject);
    }
}
