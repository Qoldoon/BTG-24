using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;
    public List<GameObject> slots = new ();
    public int current;
    bool canReload;
    public ProgressBar reloadBar;

    // Start is called before the first frame update
    void Start()
    {
        Equip(current);
        canReload = slots[0].tag == "AmmoPouch" || slots[1].tag == "AmmoPouch" || slots[2].tag == "AmmoPouch";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Equip(0); return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Equip(1); return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Equip(2); return; }
        if (Input.GetKeyDown(KeyCode.R)) { Reload(); return; }

    }
    void Equip(int item)
    {
        if (slots.Count < item + 1) return;
        current = item; 
        Settle();
    }

    Item GetItem(int item)
    {
        return slots[item].GetComponent<Item>();
    }

    public Item GetCurrent()
    {
        return GetItem(current);
    }

    public void Add(GameObject go)
    {
        GameObject item = Instantiate(go, transform);
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
            slot.SetActive(slots[current] == slot);
        }
    }
    void Reload()
    {
        // if (!canReload) return; 
        // if (!items[current].isGun) return;
        // StartCoroutine(ReloadTimer());
    }

    // IEnumerator ReloadTimer()
    // {
    //     // reloadBar?.gameObject.SetActive(true);
    //     // reloadBar?.FillInTime(items[current].reload);
    //     // yield return new WaitForSeconds(items[current].reload);
    //     // reloadBar?.gameObject.SetActive(false);
    //     // items[current].go.GetComponent<Shoot>().ammoCount = items[current].ammo;
    // }
}

