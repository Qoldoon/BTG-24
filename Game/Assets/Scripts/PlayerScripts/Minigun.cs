
public class Minigun : Weapon
{
    public override string Description { get; set; } = 
        "A minigun \n" +
        "Low accuracy \n" +
        "Normal damage \n" +
        "high capacity and fire-rate \n";
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Mini-gun";
    }
}