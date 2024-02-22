using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwither : MonoBehaviour
{
    public int item;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject current;

    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) { item = 1; return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { item = 2; return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { item = 3; return; }
        Equip(item);
        return;
    }
    void Equip(int item)
    {
        switch (item)
        {
            case 1:
                current.SetActive(false);
                slot1.SetActive(true);
                current = slot1;
                break;
            case 2:
                current.SetActive(false);
                slot2.SetActive(true);
                current = slot2;
                break;
            case 3:
                current.SetActive(false);
                slot3.SetActive(true);
                current = slot3;
                break;
        }

    }
}
