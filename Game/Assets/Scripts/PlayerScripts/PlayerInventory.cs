using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey = false;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    //public GameObject current;
    public int current;
    public static Item[] items;
    bool canReload = false;
    public ProgressBar reloadBar;
    public ItemSelection itemSlots;

    // Start is called before the first frame update
    void Start()
    {
        var i = SelectedItems.selectedItems;
        if (i is not null)
        {
            slot1 =
                Instantiate(i[0], transform.position + new Vector3(0.3f, 0.3f, -1), Quaternion.identity) as GameObject;
            slot1.transform.parent = transform;
            slot2 =
                Instantiate(i[1], transform.position + new Vector3(0.3f, 0.3f, -1), Quaternion.identity) as GameObject;
            slot2.transform.parent = transform;
            slot3 =
                Instantiate(i[2], transform.position + new Vector3(0.3f, 0.3f, -1), Quaternion.identity) as GameObject;
            slot3.transform.parent = transform;
        }



        items = new Item[3];
        items[0] = new Item(slot1);
        items[1] = new Item(slot2);
        items[2] = new Item(slot3);
        canReload = items[0].go.tag == "AmmoPouch" || items[1].go.tag == "AmmoPouch" || items[2].go.tag == "AmmoPouch";
        current = 0;
        Equip(current);
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
        if (!items[item].isEquipable) return;
        items[current].go.SetActive(false);
        items[item].go.SetActive(true);
        current = item;
        if (itemSlots is null) return;
        itemSlots.selectSlot(item);
    }
    void Reload()
    {
        if (!canReload) return; 
        if (!items[current].isGun) return;
        StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer()
    {
        reloadBar?.gameObject.SetActive(true);
        reloadBar?.FillInTime(items[current].reload);
        yield return new WaitForSeconds(items[current].reload);
        reloadBar?.gameObject.SetActive(false);
        items[current].go.GetComponent<Shoot>().ammoCount = items[current].ammo;
    }
}

public class Item : Object
{
    public GameObject go;
    public int ammo;
    public bool isGun = false;
    public bool isGrenade = false;
    public bool isEquipable = false;
    public float reload;
    public Item(GameObject gameObject)
    {
        Shoot script = gameObject.GetComponent<Shoot>();
        Throw script2 = gameObject.GetComponent<Throw>();
        go = gameObject;
        isGun = script != null;
        isGrenade = script2 != null;
        if (isGun)
        {
            ammo = script.ammoCount;
            reload = script.reloadTime;
        }
        if (isGrenade)
        {

        }
        isEquipable = isGun || isGrenade;
    }
}

