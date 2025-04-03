using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public UISlot[] slots;
    private int current;

    private void Start()
    {
        slots = GetComponentsInChildren<UISlot>();
    }

    public void SetIcons(PlayerInventory playerInventory)
    {
        for (int i = 0; i < playerInventory.slots.Count; i++)
        {
            slots[i].icon.sprite = playerInventory.slots[i].itemIcon;
            slots[i].icon.enabled = true;
        }
    }
    public void SetIcon(int index,  Sprite sprite)
    {
        slots[index].icon.sprite = sprite;
        slots[index].icon.enabled = true;
    }

    public void ClearIcon(int index)
    {
        slots[index].icon.sprite = null;
        slots[index].icon.enabled = false;
    }
    public void Select(int index)
    {
        Debug.Log($"{index} {slots[current]} now ");
        slots[current].image.color = Color.gray;
        slots[index].image.color = Color.white;
        current = index;
    }
}
