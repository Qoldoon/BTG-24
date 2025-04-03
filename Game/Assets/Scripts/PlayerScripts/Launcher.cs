public class Launcher : Weapon
{
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Launcher";
    }
}