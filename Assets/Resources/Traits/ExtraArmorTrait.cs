using UnityEngine;

public class ExtraArmorTrait : Trait
{
    public ExtraArmorTrait()
    {
        Color = Color.red; // Example color
        Id = 1; // Example ID
        Description = "Fat to increase health at cost of speed";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.maxHealth += 5;
        drone.speed -= 1;
    }
}