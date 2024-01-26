using UnityEngine;

public class ArmorPlatingTrait : Trait
{
    public ArmorPlatingTrait()
    {
        Color = Color.gray; // Example color
        Id = 7; // Example ID
        Description = "Increase armor at the cost of speed";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.armor += 1;
        drone.speed *= 0.8f;
    }
}