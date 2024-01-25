using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneImage : MonoBehaviour
{

    public Drone linkedDrone;
    public bool wasSelected;
    
    // Start is called before the first frame update

    public void AddLinkedDrone(Drone drone) {
        linkedDrone = drone;
    }

    public void WasSelected() {
        transform.root.GetComponent<BreedingMenu>().SelectToBreed(this);
    }
}
