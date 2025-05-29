using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoPouch : Item
{
    [SerializeField] private int reloads = 60;
    public override string Description { get; set; } =
        "Spare ammunition \n" +
        "Allows reloading for all weapons \n";
    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        transform.localPosition = new Vector3(0f, 0f, 0f);
        PlayerInventory.reloads = reloads;
        PlayerInventory.canReload = true;
    }

    public override void OnRemove(int index)
    { 
        PlayerInventory.reloads = 0;
        PlayerInventory.canReload = false;
        base.OnRemove(index);
    }

    public override void Equip()
    {
        PlayerInventory.reloadIndicator?.Display((float)PlayerInventory.reloads/reloads);
    }

    public override void UnEquip()
    {
        PlayerInventory.reloadIndicator?.Clear();
    }
}
