using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPouch : Item
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        PlayerInventory.canReload = true;
    }

    public override void OnRemove()
    { 
        PlayerInventory.canReload = false;
    }
}
