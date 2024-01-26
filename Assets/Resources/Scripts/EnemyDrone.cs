using UnityEngine;

public class EnemyDrone : Drone
{

    // public override void Update()
    // {
    //     attackList = Camera.main.GetComponent<Game>().livingAllies; //too expensive
    //     base.Update();

    // }

    public override void Die() {
        Game g = Camera.main.GetComponent<Game>();

        //remove self from tracking list
        g.livingEnemies.Remove(gameObject.GetComponent<Drone>());

        //remove self from all allies attack list
        foreach (Drone d in g.livingAllies) {
            d.attackList.Remove(this.GetComponent<Drone>());
        }

        Debug.LogWarning("enemy died");
        FindObjectOfType<AudioManager>().Play("DroneDeath");

        Destroy(gameObject);
    }
}