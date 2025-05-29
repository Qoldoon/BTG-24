using System;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        if (!Input.GetKey(KeyCode.Q)) return;
        var inv = other.gameObject.GetComponent<PlayerInventory>();
        inv.Add(item);
        Destroy(gameObject);
    }
}
