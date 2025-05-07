public class Sniper : Weapon
{
    public override string Description { get; set; } = 
        "A Sniper rifle \n" +
        "high accuracy and projectile velocity \n" +
        "High damage, armor piercing \n" +
        "5 shot capacity, low fire-rate \n";
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Sniper";
    }
}