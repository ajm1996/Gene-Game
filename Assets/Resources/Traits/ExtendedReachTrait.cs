using UnityEngine;

public class ExtendedReachTrait : Trait
{
    public ExtendedReachTrait()
    {
        Color = Color.blue; // Example color
        Id = 2; // Example ID
        Description = "Claws extend for farther attack range";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.AttackDistance += 1.1f;
    }
}