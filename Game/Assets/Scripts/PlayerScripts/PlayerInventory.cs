using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    List<string> keys = new();
    public Inventory slots = new (3);
    public int current;
    public bool canReload;
    public int reloads = 60;
    [NonSerialized]
    public List<GameObject> Boxes = new ();
    public float multiplier = 1;
    [NonSerialized]
    public PlayerCanvas canvas;
    [NonSerialized]
    public PlayerUI playerUI;
    [NonSerialized]
    public Indicator reloadIndicator;

    public GameObject pcik;
    
    void Start()
    {
        canvas = gameObject.GetComponentInChildren<PlayerCanvas>();
        playerUI = gameObject.GetComponentInChildren<PlayerUI>();
        reloadIndicator = gameObject.GetComponentInChildren<Indicator>();
        var selectedItems = GameObject.Find("SelectedItems")?.GetComponent<SelectedItems>();
        if (selectedItems == null) return;
        foreach (var item in selectedItems.selectedItems)
        {
            Add(item.gameObject);
        }
        
        Equip(current);
    }

    public void Equip(int item)
    {
        if (!slots.Exists(item)) return;
        if(slots.Exists(current))
            slots[current].UnEquip();
        current = item; 
        slots[current].Equip();
        Settle();
    }

    public bool IsUsable(out IUsable usableItem)
    {
        usableItem = null;
        if (slots != null && current >= 0 && slots.Exists(current))
        {
            return slots[current].TryGetComponent(out usableItem);
        }
        return false;
    }

    public void Toss(Vector2 direction)
    {
        var item = slots[current];

        var pickup = Instantiate(pcik, transform.position, Quaternion.identity);
        pickup.GetComponent<Pickup>().item = item.gameObject;
        item.transform.SetParent(pickup.transform);
        var p = pickup.AddComponent<TossPhysics>();
        p.direction = direction;
        
        Remove(current, false);
    }
    public void Add(GameObject go)
    {
        if (!go.TryGetComponent(out Item item)) return;
        if (go.scene.IsValid())
        {
            go.transform.SetParent(transform, false);
        }
        else
        {
            item = Instantiate(item, transform);
        }
        
        if (slots.Count < 3)
        {
            Add(item);
            return;
        }
        Replace(current, item);
    }

    private void Add(Item item)
    {
        var index = slots.Add(item);
        item.OnAdd(this, index);
        if(index == current)
            slots[current].Equip();
        Settle();
    }

    private void Replace(int index, Item item)
    {
        var slot = slots[index];
        slot.OnRemove(index);
        Destroy(slot.gameObject);
        slots[index] = item;
        item.OnAdd(this, index);
        if(index == current)
            slots[current].Equip();
        Settle();
    }

    private void Remove(int index, bool destroy = true)
    {
        var slot = slots[index];
        slot.OnRemove(index);
        if(destroy) Destroy(slot.gameObject);
        slots.RemoveAt(index);
        Settle();
    }

    private void Settle()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(slots[current] == slot);
        }
        if(slots.Exists(current))
            playerUI?.Select(current);
        else playerUI?.Unselect();
    }

    public void AddKey(string key)
    {
        keys.Add(key);
    }
    
    public bool HasKey(string key)
    {
        return keys.Contains(key);
    }

    public void Amplify()
    {
        multiplier = 2;
    }

    public void DeAmplify()
    {
        multiplier = 1;
    }

    public void Reload(int reloadCost)
    {
        reloads -= reloadCost;
        canReload = reloads > 0;
        UpdateAmmoPacks(reloads / 60f);
    }

    private void UpdateAmmoPacks(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        int currentStage = Mathf.FloorToInt(percentage * 6);
        
        foreach (var box in Boxes)
        {
            if (box != null)
            {
                box.SetActive(true);
                box.transform.localScale = new  Vector3(0.25f, 0.2f, 1);
            }
        }
        
        if (percentage <= 0)
        {
            foreach (var pack in Boxes.Where(pack => pack != null))
            {
                pack.SetActive(false);
            }
        }
        else
        {
            for (int i = 2; i >= 0; i--)
            {
                if (currentStage <= 2 * i)
                {
                    if (Boxes[i] != null) Boxes[i].SetActive(false);
                }
                else if (currentStage <= 2 * i + 1)
                {
                    if (Boxes[i] != null)
                    {
                        Boxes[i].transform.localScale = new  Vector3(0.25f, 0.1f, 1);
                    }
                }
            }
        }
    }
}


public class Inventory : IEnumerable<Item>
{
    private Item[] Items;
    public int Count;

    public Inventory(int size)
    {
        Items = new Item[size];
    }
    
    public int Add(Item item)
    {
        if(!FindFirstFreeSlot(out var index)) return 0;
        Items[index] = item;
        Count++;
        return index;
    }
    
    public void RemoveAt(int index)
    {
        Items[index] = null;
        Count--;
    }

    public bool Exists(int index)
    {
        return Items[index] != null;
    }

    private bool FindFirstFreeSlot(out int index)
    {
        index = -1;
        for (var i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null) continue;
            index = i;
            return true;
        }

        return false;
    }
    
    public Item this[int index]
    {
        get
        {
            if (index < 0 || index >= Items.Length)
                throw new IndexOutOfRangeException("Index is out of range.");
            return Items[index];
        }
        set
        {
            if (index < 0 || index >= Items.Length)
                throw new IndexOutOfRangeException("Index is out of range.");
            Items[index] = value;
        }
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return Items.Where(item => item != null).GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
