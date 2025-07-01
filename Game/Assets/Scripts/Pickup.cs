using System;
using UnityEngine;

public class Pickup : Interactable
{
    public GameObject item;
    

    void Start()
    {
        if (item == null)
        {
            item = GetComponentInChildren<Item>()?.gameObject;
        }
        else if (GetComponentInChildren<Item>() == null)
        {
            Instantiate(item, transform);
        }
        if(item == null) Destroy(gameObject);
    }

    protected override void Interact()
    {
        if(!_ready) return;
        _player.playerInventory.Add(item);
        _player.Interact -= Interact;
        Destroy(gameObject);
    }

    
}
