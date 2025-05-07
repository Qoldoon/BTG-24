using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPouch : Item
{
    public override string Description { get; set; } =
        "Spare ammunition \n" +
        "Allows reloading for all weapons \n";
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        transform.localPosition = new Vector3(0f, 0f, 0f);
        PlayerInventory.canReload = true;
    }

    public override void OnRemove()
    { 
        PlayerInventory.canReload = false;
    }
}
