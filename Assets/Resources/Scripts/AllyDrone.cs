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
        SetAllyCollision(false);
        base.Start();
        //hideHealthbar();
    }

    public override void Update()
    {
        if (breedingTarget != null) {
            MoveTo(breedingTarget);
            Physics2D.IgnoreCollision(breedingTarget.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
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
        if (!BreedingDeath) {
            FindObjectOfType<AudioManager>().Play("DroneDeath");
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

    public void SetAllyCollision(bool canCollide) {
        foreach(AllyDrone ad in g.livingAllies) {
            if (!canCollide) Physics2D.IgnoreCollision(ad.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            else Physics2D.IgnoreCollision(ad.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
    }

}