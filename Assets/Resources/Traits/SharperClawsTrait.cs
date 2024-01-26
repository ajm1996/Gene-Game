using UnityEngine;

public class SharperClawsTrait : Trait
{
    public SharperClawsTrait()
    {
        Color = Color.green; // Example color
        Id = 3; // Example ID
        Description = "Sharpen claws for extra damage";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.damage += 5;
    }
}