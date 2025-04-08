using System;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Behaviour b;
    private void Start()
    {
        b = GetComponentInParent<Behaviour>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        b.Detection(other);
    }
}
