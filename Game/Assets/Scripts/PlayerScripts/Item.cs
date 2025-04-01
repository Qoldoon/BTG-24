using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected PlayerInventory PlayerInventory;
    public virtual void OnAdd(PlayerInventory inventory)
    {
        PlayerInventory = inventory;
    }
    public virtual void OnRemove()
    {
        PlayerInventory = null;
    }
    public virtual void Equip()
    {
        
    }
    public virtual void UnEquip()
    {
        
    }
}
