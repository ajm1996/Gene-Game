using UnityEngine;

public class AllyDrone : Drone
{

     public override void Start()
    {
        base.Start();
        //hideHealthbar();
    }

    // public override void Update()
    // {
    //     attackList = Camera.main.GetComponent<Game>().livingEnemies; //too expensive
    //     base.Update();

    // }

    public override void Die() {
        Game g = Camera.main.GetComponent<Game>();

        //remove self from tracking list
        g.livingAllies.Remove(gameObject.GetComponent<Drone>());

        //remove self from all allies attack list
        foreach (Drone d in g.livingEnemies) {
            d.attackList.Remove(this.GetComponent<Drone>());
        }

        Destroy(gameObject);
    }
}