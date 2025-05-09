using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    List<string> keys = new();
    public List<Item> slots = new ();
    public int current;
    public bool canReload;
    public float multiplier = 1;
    [NonSerialized]
    public PlayerCanvas canvas;
    [NonSerialized]
    public PlayerUI playerUI;
    
    void Start()
    {
        canvas = gameObject.GetComponentInChildren<PlayerCanvas>();
        playerUI = gameObject.GetComponentInChildren<PlayerUI>();
        var selectedItems = GameObject.Find("SelectedItems")?.GetComponent<SelectedItems>();
        if (selectedItems == null) return;
        foreach (var item in selectedItems.selectedItems)
        {
            Add(item.gameObject);
        }
        
        Equip(current);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Equip(0); return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Equip(1); return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Equip(2); return; }
    }
    void Equip(int item)
    {
        if (slots.Count < item + 1) return;
        slots[current].UnEquip();
        current = item; 
        slots[current].Equip();
        Settle();
    }

    public bool IsUsable(out IUsable usableItem)
    {
        usableItem = null;
        if (slots != null && current >= 0 && current < slots.Count)
        {
            return slots[current].TryGetComponent(out usableItem);
        }
        return false;
    }

    public void Add(GameObject go)
    {
        if (!go.TryGetComponent(out Item item)) return;
        item = Instantiate(item, transform);
        
        if (slots.Count < 3)
        {
            slots.Add(item);
            item.OnAdd(this);
            Settle();
            return;
        }
        slots[current].OnRemove();
        Destroy(slots[current].gameObject);
        slots[current] = item;
        item.OnAdd(this);
        Settle();
    }

    void Settle()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(slots[current] == slot);
            playerUI?.Select(current);
        }
    }

    public void addKey(string key)
    {
        keys.Add(key);
    }
    public bool hasKey(string key)
    {
        return keys.Contains(key);
    }

    public void Amplify()
    {
        multiplier = 2;
    }

    public void deAmplify()
    {
        multiplier = 1;
    }
}

