using System;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    public GameObject item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var x = GetComponentInChildren<Item>();
        if(x != null) 
            item = x.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        var inv = other.gameObject.GetComponent<PlayerInventory>();
        inv.Add(item);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
