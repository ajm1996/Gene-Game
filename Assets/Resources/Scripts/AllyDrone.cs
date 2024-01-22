using UnityEngine;

public class AllyDrone : Drone
{

    public override void Update()
    {
        attackList = Camera.main.GetComponent<Game>().livingEnemies;
        base.Update();

    }
}