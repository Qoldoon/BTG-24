using Pathfinding;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    Transform target;
    AIDestinationSetter setter;
    public float visionRange = 12f;
    public float visionAngle = 100f;

    void Start()
    {
        target = transform.GetChild(0);
        setter = GetComponent<AIDestinationSetter>();
        setter.target = target;
    }
    
    void Update()
    {

    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        Vector2 directionToTarget = (other.transform.position - transform.position).normalized;
        float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

        if (angleToTarget <= visionAngle / 2)
        {
            //TODO: raycast to check for obstacles
            
        }
    }
}
