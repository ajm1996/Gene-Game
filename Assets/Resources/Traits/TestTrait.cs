using UnityEngine;

public class TestTrait : Trait
{
    public TestTrait()
    {
        Color = Color.red; // Example color
        Id = 1; // Example ID
    }

    public override void ApplyEffect(Drone drone)
    {
        //drone.speed += 1;
        drone.maxHealth += 2;
    }
}