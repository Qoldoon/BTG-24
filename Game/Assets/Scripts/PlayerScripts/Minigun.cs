
public class Minigun : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Mini-gun";
    }
}