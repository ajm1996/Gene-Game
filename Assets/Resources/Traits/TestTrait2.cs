using UnityEngine;

public class TestTrait2 : Trait
{
    public TestTrait2()
    {
        Color = Color.blue; // Example color
        Id = 2; // Example ID
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.speed += 10;
        drone.health -= 3 ;
    }
}