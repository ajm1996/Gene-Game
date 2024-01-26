using UnityEngine;

public class AdrenalineTrait : Trait
{
    public AdrenalineTrait()
    {
        Color = Color.yellow; // Example color
        Id = 4; // Example ID
        Description = "Increase adrenaline glands for better attack speed";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.AttackSpeed *= 0.8f;
    }
}