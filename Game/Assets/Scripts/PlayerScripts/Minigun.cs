
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
}