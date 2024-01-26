using UnityEngine;

public class VampireFangsTrait : Trait
{
    public VampireFangsTrait()
    {
        Color = Color.green; // Example color
        Id = 6; // Example ID
        Name = "Vampire Fangs";
        Description = "Heal for an amount at each attack";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.lifesteal += 2;
    }
}