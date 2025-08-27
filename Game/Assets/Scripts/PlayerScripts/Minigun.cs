
using UnityEngine;
using UnityEngine.UIElements;

public class Minigun : Weapon
{
    public override string Description { get; set; } = 
        "A minigun \n" +
        "Low accuracy \n" +
        "Normal damage \n" +
        "high capacity and fire-rate \n";
    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        name = "Mini-gun";
    }

    public override void Equip()
    {
        base.Equip();
        _time += 0.5f;
        PlayerInventory.GetComponent<PlayerController>().MultiplySpeed(0.5f, 10);
    }
}