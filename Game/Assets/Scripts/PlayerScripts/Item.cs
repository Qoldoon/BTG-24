using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour
{
    protected PlayerInventory PlayerInventory;
    public Sprite itemIcon;
    public virtual void OnAdd(PlayerInventory inventory)
    {
        PlayerInventory = inventory;
        transform.localPosition = new Vector3(0f, 0.3f, -1f);
        transform.localRotation = Quaternion.identity;
        if (itemIcon != null)
            PlayerInventory.playerUI?.SetIcons(PlayerInventory);
    }
    public virtual void OnRemove()
    {
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
