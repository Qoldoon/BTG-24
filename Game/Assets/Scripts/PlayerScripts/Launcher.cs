public class Launcher : Weapon
{
    public override string Description { get; set; } = 
        "A Rocket launcher \n" +
        "High damage with explosion \n" +
        "Dangerous to user \n" +
        "3 shot capacity, slow reload \n";
    public override void OnAdd(PlayerInventory inventory)
    {
        base.OnAdd(inventory);
        name = "Launcher";
    }
}