using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;
    public List<Item> slots = new ();
    public int current;
    public bool canReload;
    [NonSerialized]
    public PlayerCanvas canvas;
    public PlayerUI playerUI;
    
    void Start()
    {
        canvas = gameObject.GetComponentInChildren<PlayerCanvas>();
        playerUI = gameObject.GetComponentInChildren<PlayerUI>();
        foreach (var item in SelectedItems.selectedItems)
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

    public bool isUsable(out IUsable usableItem)
    {
        return slots[current].TryGetComponent(out usableItem);
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
}

