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
    private bool IsAggro => Sighting.IsRecent(_sightings.PlayerSighting(), 0.1f);
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
        if (Input.GetKey(KeyCode.U))
        {
            var sighting = new Sighting
            {
                Target = GameObject.FindWithTag("Player"),
                Velocity = new Vector3(3, 0, 0),
                Position = new Vector3(0, 5, 0),
                TimeSeen = Time.time-0.1f
            };
            _sightings.TryAddSighting(sighting);
        } //Delete this
        Rotate();
        React();
        SetMoveTarget(movementTarget.position);
    }

    private void SetAimTarget(Vector3 targetPosition)
    {
        aimTarget.position = targetPosition;
    }

    private void SetMoveTarget(Vector3 targetPosition)
    {
        movementTarget.position = targetPosition;
        Collider2D[] hits = Physics2D.OverlapCircleAll(movementTarget.position, 1f);

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            
            Vector2 direction = (movementTarget.position - hit.transform.position).normalized;
            
            float overlapDistance = 1f - Vector2.Distance(movementTarget.position, hit.transform.position);
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
    // private void React()
    // {
    //     if (_sightings.Count == 0)
    //         return;
    //
    //     var sighting = Decide();
    //     if (sighting.Target == null || sighting.Target.Equals(null))
    //         return;
    //     
    //     if (sighting.Target.CompareTag("Player"))
    //     {
    //         if (IsRecent(sighting, 0.1f))
    //             AttackPlayer(sighting);
    //         else if (IsRecent(sighting, 0.2f))
    //             Follow(sighting);
    //         else LookAround(sighting);
    //     }
    //     else if (sighting.Target.CompareTag("Enemy"))
    //     {
    //         FollowAlly(sighting);
    //     }
    //     else if (sighting.Target.CompareTag("Weapon"))
    //     {
    //         Investigate(sighting);
    //     }
    // }

    

    private Sighting Decide()
    {
        Sighting bestCandidate = null;

        foreach (var sighting in _sightings)
        {
            if (sighting.Target == null || sighting.Target.Equals(null)) 
                continue;
            
            if (bestCandidate == null)
            {
                bestCandidate = sighting;
                continue;
            }
            
            if (sighting.Target.CompareTag("Player") && !bestCandidate.Target.CompareTag("Player"))
            {
                bestCandidate = sighting;
            }
            else if (sighting.Target.CompareTag("Weapon") && !(bestCandidate.Target.CompareTag("Player") && Sighting.IsRecent(bestCandidate, 0.1f)))
            {
                bestCandidate = sighting;
            }
            else if (sighting.Target.CompareTag("Enemy") && sighting.Target.GetComponent<Behaviour>().IsAggro && !bestCandidate.Target.CompareTag("Player") && !bestCandidate.Target.CompareTag("Weapon"))
            {
                bestCandidate = sighting;
            }
            else if (sighting.Target.CompareTag(bestCandidate.Target.tag) && sighting.TimeSeen > bestCandidate.TimeSeen)
            {
                bestCandidate = sighting;
            }
        }

        return bestCandidate;
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
                    Velocity = hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.linearVelocity : Vector3.zero,
                    Position = hit.point,
                    TimeSeen = Time.time
                };
                // if(!hit.collider.CompareTag("Player"))
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
                TimeSeen = Time.time - 11f
            };
            _sightings.TryAddSighting(sighting);
        }
    }

    // private void TryAddSighting(Sighting sighting)
    // {
    //     var existingSighting = _sightings.Find(s => s.Equals(sighting));
    //     if (existingSighting == null)
    //         _sightings.Add(sighting);
    //     else
    //     {
    //         existingSighting.TimeSeen = sighting.TimeSeen;
    //         existingSighting.Velocity = sighting.Velocity;
    //         existingSighting.Position = sighting.Position;
    //     }
    // }

    private void LookAround(Sighting sighting)
    {
        if (Vector2.Distance(movementTarget.position, transform.position) > 0.2f) return;
        SetMoveTarget(sighting.Position + sighting.Velocity * 0.5f);

        
        SetAimTarget(transform.position + RightVector(sighting.Position - transform.position));
    }

    public static Vector3 RightVector(Vector3 forward)
    {
        return new Vector2(forward.normalized.y, -forward.normalized.x);
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

    public void Follow(Sighting sighting)
    {
        SetMoveTarget(sighting.Position);
        SetAimTarget(movementTarget.position + sighting.Velocity * 0.5f);
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
