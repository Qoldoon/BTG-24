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
    private bool isAggro;
    private List<Sighting> _sightings = new ();

    
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
        if (_sightings.Count == 0)
            return;
        _sightings.RemoveAll(s => Time.time - s.TimeSeen > 12);

        var sighting = Decide();
        if (sighting.Target == null || sighting.Target.Equals(null))
            return;
        Debug.Log($"{name} {sighting.Target.name}");
        if (sighting.Target.CompareTag("Player"))
        {
            if (IsRecent(sighting, 0.1f))
                AttackPlayer(sighting);
            else if (IsRecent(sighting, 0.2f))
                Follow(sighting);
            else LookAround(sighting);
        }
        else if (sighting.Target.CompareTag("Enemy"))
        {
            FollowAlly(sighting);
        }
        else if (sighting.Target.CompareTag("Weapon"))
        {
            Investigate(sighting);
        }
    }

    

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
            else if (sighting.Target.CompareTag("Weapon") && !(bestCandidate.Target.CompareTag("Player") && IsRecent(bestCandidate, 0.1f)))
            {
                bestCandidate = sighting;
            }
            else if (sighting.Target.CompareTag("Enemy") && sighting.Target.GetComponent<Behaviour>().isAggro && !bestCandidate.Target.CompareTag("Player") && !bestCandidate.Target.CompareTag("Weapon"))
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

    private bool IsRecent(Sighting sighting, float threshold)
    {
        return Mathf.Abs(sighting.TimeSeen - Time.time) <= threshold;
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
                TryAddSighting(sighting);
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
            TryAddSighting(sighting);
        }
    }

    private void TryAddSighting(Sighting sighting)
    {
        var existingSighting = _sightings.Find(s => s.Equals(sighting));
        if (existingSighting == null)
            _sightings.Add(sighting);
        else
        {
            existingSighting.TimeSeen = sighting.TimeSeen;
            existingSighting.Velocity = sighting.Velocity;
            existingSighting.Position = sighting.Position;
        }
    }

    private void LookAround(Sighting sighting)
    {
        return;
        SetAggro(false);
        if (Vector2.Distance(movementTarget.position, transform.position) > 0.2f) return;
        SetMoveTarget(sighting.Position + sighting.Velocity * 0.5f);
        
        Vector3 right = new Vector2(sighting.Velocity.normalized.y, -sighting.Velocity.normalized.x).normalized;
        Vector3 left = new Vector2(-sighting.Velocity.normalized.y, sighting.Velocity.normalized.x).normalized;
        
        Vector2 rightPosition =  sighting.Position + (right * 2);
        Vector2 leftPosition =  sighting.Position + (left * 2);
        
        float timeCycle = Time.time % 3;
        if (timeCycle < 1f)
            aimTarget.position = rightPosition;
        else if (timeCycle < 2f)
            aimTarget.position = leftPosition;
        else if (timeCycle < 3f)
        {
            sighting.Position += sighting.Velocity;
            SetMoveTarget(aimTarget.position);
        }
    }
    private void AttackPlayer(Sighting sighting)
    {
        SetAggro(true);   
        SetAimTarget(sighting.Position);
        if (Vector3.Distance(transform.position, sighting.Target.transform.position) >= 8.4f)
            KeepDistance(sighting.Position);
        else StandStill();
        Shoot();
    }

    private void Investigate(Sighting sighting)
    {
        SetAimTarget(sighting.Position);
        SetMoveTarget(sighting.Position);
    }

    private void FollowAlly(Sighting sighting)
    {
        var b = sighting.Target.GetComponent<Behaviour>();
        if (b.isAggro)
        {
            SetMoveTarget(b.aimTarget.position);
            SetAimTarget(b.aimTarget.position);
        }
    }

    private void Follow(Sighting sighting)
    {
        if (sighting.Velocity != Vector3.zero)
        {
            SetMoveTarget(sighting.Position);
            SetAimTarget(movementTarget.position + sighting.Velocity * 0.5f);
        }
    }

    private void SetAggro(bool state)
    {
        isAggro = state;
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
