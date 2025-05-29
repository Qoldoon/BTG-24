public class Grenade : Weapon
{
    public override string Description { get; set; } = 
        "A Grenade launcher \n" +
        "Normal damage with ability to disable shields \n" +
        "Dangerous to user \n" +
        "3 shot capacity, low range \n";
    public override void OnAdd(PlayerInventory inventory, int index)
    {
        base.OnAdd(inventory, index);
        name = "Grenade";
    }
}