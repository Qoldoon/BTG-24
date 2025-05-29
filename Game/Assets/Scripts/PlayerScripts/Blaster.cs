using UnityEngine;

public class Blaster : Weapon
{
    public override string Description { get; set; } = 
        "A simple blaster \n" +
        "Normal damage \n" +
        "6 shot capacity \n";
    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        name = "Blaster";
    }
}
