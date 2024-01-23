using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingMenu : MonoBehaviour
{

    public GameObject scrollMenuContent;
    public GameObject droneImage;

    // Start is called before the first frame update
    void Start()
    {
        //Get list of living ally drones and populate scroll view with it
        List<GameObject> livingAllies = GameObject.Find("Main Camera").GetComponent<Game>().livingAllies;
        SetupDroneScrollView(livingAllies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Breed() {
        Debug.Log("Clicked breed");
    }

    void SetupDroneScrollView(List<GameObject> livingAllies) {
        for (var i = 0; i < livingAllies.Count; i++) {
            AddDroneToScrollView();
        }
    }

    void AddDroneToScrollView() {
        GameObject drone = Instantiate(droneImage);
        drone.transform.SetParent(scrollMenuContent.transform, false);
    }
}
