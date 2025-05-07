using UnityEngine;

public class Blaster : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Blaster";
    }
}
