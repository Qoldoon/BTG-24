using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour
{
    public virtual string Description { get; set; } = "Default";
    protected PlayerInventory PlayerInventory;
    public Sprite itemIcon;
    public virtual void OnAdd(PlayerInventory inventory, int index)
    {
        PlayerInventory = inventory;
        transform.localPosition = new Vector3(0f, 0.3f, 0f);
        transform.localRotation = Quaternion.identity;
        if (itemIcon != null)
            PlayerInventory.playerUI?.SetIcon(index, itemIcon);
    }
    public virtual void OnRemove(int index)
    {
        PlayerInventory.playerUI?.ClearIcon(index);
        PlayerInventory = null;
        transform.localPosition = Vector3.zero;
    }
    public virtual void Equip()
    {

    }
    public virtual void UnEquip()
    {

    }
}
