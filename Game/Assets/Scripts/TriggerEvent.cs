using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent target;
    [SerializeField]
    private string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(targetTag)) return;
        var condition = GetComponent<TriggerCondition>();
        if (condition != null && !condition.Condition(other.gameObject))
        {
            other.GetComponent<PlayerInventory>().canvas?.CreateText("Missing key");
            return;
        }
        target.Invoke();
    }
}
