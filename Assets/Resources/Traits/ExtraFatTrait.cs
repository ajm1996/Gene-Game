using UnityEngine;

public class ExtraFatTrait : Trait
{
    public ExtraFatTrait()
    {
        Color = Color.red; // Example color
        Id = 1; // Example ID
        Name = "Extra Fat";
        Description = "Increase health at cost of speed";
    }

    public override void ApplyEffect(Drone drone)
    {
        drone.maxHealth += 50;
        drone.currentHealth += 50;
        var localScale = drone.gameObject.transform.localScale;
        drone.gameObject.transform.localScale = new Vector3(localScale.x * 1.3f, localScale.y * 1.3f);
    }
}