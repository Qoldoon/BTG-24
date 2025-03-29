using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;
    public List<Item> slots = new ();
    public int current;
    
    void Start()
    {
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
        item.transform.localPosition = new Vector3(0f, 0.3f, -1f);
        item.transform.localRotation = Quaternion.identity;
        
        if (slots.Count < 3)
        {
            slots.Add(item);
            Settle();
            return;
        }

        Destroy(slots[current]);
        slots[current] = item;
        Settle();
    }

    void Settle()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(slots[current] == slot);
        }
    }
}

