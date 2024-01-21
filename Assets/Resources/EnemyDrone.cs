using UnityEngine;

public class EnemyDrone : Drone
{

    public override void Update()
    {
        attackList = Camera.main.GetComponent<Game>().livingAllies;
        base.Update();

    }
}