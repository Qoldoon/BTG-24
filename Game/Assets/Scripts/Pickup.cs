using System;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public GameObject item;
    private PlayerController _player;
    private bool _ready;

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

    public void Interact()
    {
        if(!_ready) return;
        _player.playerInventory.Add(item);
        _player.Interact -= Interact;
        Destroy(gameObject);
    }
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if(!other.gameObject.CompareTag("Player")) return;
    //     if (!Input.GetKey(KeyCode.Q)) return; //TODO: inputs in controller
    //     var inv = other.gameObject.GetComponent<PlayerInventory>();
    //     inv.Add(item);
    //     Destroy(gameObject);
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        _ready = true;
        _player = other.gameObject.GetComponent<PlayerController>();
        _player.Interact += Interact;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        _ready = false;
        _player.Interact -= Interact;
        _player = null;
    }
}
