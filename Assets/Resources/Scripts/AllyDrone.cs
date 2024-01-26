using UnityEngine;
using UnityEngine.Rendering;

public class AllyDrone : Drone
{

    private Game g;
    private BreedingMenu bm;
    public GameObject breedingTarget;

    public override void Start()
    {
        g = Camera.main.GetComponent<Game>();
        bm = g.breedingMenu.GetComponent<BreedingMenu>();
        base.Start();
        //hideHealthbar();
    }

    public override void Update()
    {
        if (breedingTarget != null) {
            MoveTo(breedingTarget);
        }
        base.Update();

    }

    public override void Die() {
        //remove self from tracking list
        g.livingAllies.Remove(gameObject.GetComponent<Drone>());

        //remove self from all allies attack list
        foreach (Drone d in g.livingEnemies) {
            d.attackList.Remove(this.GetComponent<Drone>());
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == breedingTarget)
        {
            if(bm.breedingTarget1.linkedDrone == collision.gameObject.GetComponent<Drone>() && bm.readyToBreed) {
                bm.StartBreeding();
            }
            breedingTarget = null;
        }
    }

}