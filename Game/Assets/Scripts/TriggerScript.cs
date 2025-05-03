using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    [SerializeField]
    private UnityEvent target;
    [SerializeField]
    private string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(targetTag)) return;
        var condition = GetComponent<TriggerCondition>();
        if (condition != null && !condition.Condition(other.gameObject)) return;
        target.Invoke();
    }
}
