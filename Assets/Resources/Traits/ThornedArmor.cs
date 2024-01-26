using UnityEngine;

public class ThornedArmorTrait : Trait
{
    public ThornedArmorTrait()
    {
        Color = Color.cyan; // Example color
        Id = 8; // Example ID
        Name = "Thorned Armor";
        Description = "Deal damage back to enemies when hit";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.thorns += 2;
    }
}