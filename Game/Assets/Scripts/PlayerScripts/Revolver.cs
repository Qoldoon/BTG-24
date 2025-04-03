using UnityEngine;

public class Revolver : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Revolver";
    }
}
