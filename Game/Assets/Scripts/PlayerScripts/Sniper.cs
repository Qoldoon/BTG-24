public class Sniper : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Sniper";
    }
}