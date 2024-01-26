using UnityEngine;

public class ThickThighsTrait : Trait
{
    public ThickThighsTrait()
    {
        Color = Color.black; // Example color
        Id = 5; // Example ID
        Description = "Thicken thighs for higher movement speed";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.speed += 5;
    }
}