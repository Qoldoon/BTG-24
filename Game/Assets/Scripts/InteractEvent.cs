using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractEvent : Interactable
{
    [SerializeField]
    private UnityEvent @event;
    protected override void Interact()
    {
        var condition = GetComponent<TriggerCondition>();
        if (condition != null && !condition.Condition(_player.gameObject))
        {
            _player.playerInventory.canvas?.CreateText("Missing key");
            return;
        }
        @event.Invoke();
    }
}
