using System;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private EnemyBehaviour b;
    private void Start()
    {
        b = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        b.Detection(other);
    }
}
