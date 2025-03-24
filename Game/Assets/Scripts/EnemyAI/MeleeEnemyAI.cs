using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeEnemyAI : MonoBehaviour
{
    public Transform target;
    public bool followEnabled = true;
    public float activateDistance = 20f;
    private AIPath path;
    public int mainmenu;
    private AIDestinationSetter setter;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        setter = GetComponent<AIDestinationSetter>();
        setter.target = target;
        path = GetComponent<AIPath>();
    }

    void Update()
    {
        if (TargetInDistance())
        {
            setter.enabled = true;
        }
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var controller = collision.GetComponent<PlayerController>();
            controller.Hit(transform.position, 20, 0);
            Destroy(gameObject);
        }
    }
}
