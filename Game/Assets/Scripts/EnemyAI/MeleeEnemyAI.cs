using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeEnemyAI : MonoBehaviour
{
    public Transform target;
    public bool followEnabled = true;
    public float activateDistance = 20f;
    private Path path;
    public int mainmenu;
    void Update()
    {
        if (TargetInDistance())
        {
            GetComponent<AIDestinationSetter>().enabled = true;
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
            PlayerStats.SubtractHitPoint();

            if (PlayerStats.GetHitPoints() < 1)
            {
                PlayerStats.Die(mainmenu);

            }
            Destroy(gameObject);

        }
    }
}
