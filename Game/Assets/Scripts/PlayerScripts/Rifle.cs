
public class Rifle : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Rifle";
    }
}