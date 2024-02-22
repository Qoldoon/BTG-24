using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    GameObject[] slots;
    int current = 0;

    private void Awake()
    {
        slots = new GameObject[3];
        slots[0] = transform.Find("Slot1").gameObject;
        slots[1] = transform.Find("Slot2").gameObject;
        slots[2] = transform.Find("Slot3").gameObject;

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void selectSlot(int number)
    {
        slots[current].GetComponent<Image>().color = new Color(236f / 255f, 179f / 255f, 28f / 255f);
        slots[number].GetComponent<Image>().color = new Color(236f / 255f, 179f / 255f, 1);
        current = number;
    }
}
