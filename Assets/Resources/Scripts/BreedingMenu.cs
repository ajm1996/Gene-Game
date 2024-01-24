using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingMenu : MonoBehaviour
{

    public GameObject scrollMenuContent;
    public GameObject droneImage;
    public List<GameObject> allyDroneList;
    public GameObject selectOneContent;
    private bool selectOneSelecting;
    public GameObject selectTwoContent;
    private bool selectTwoSelecting;
    public GameObject firstSelectedDrone;
    public GameObject secondSelectedDrone;
    private Game gameComponent;

    // Start is called before the first frame update
    void Start()
    {
        //Get list of living ally drones and populate scroll view with it
        if (gameComponent != null)
        {
            SetupDroneScrollView(gameComponent.livingAllies);
        }

        //Preset select buttons to not be selecting
        DisableSelection();
    }

    // Update is called once per frame
    void Update()
    {
        //While breeding menu is active, check if any drone in the list has been selected
        foreach (var drone in allyDroneList) {
            if (drone.GetComponent<DroneImage>().wasSelected) {
                if (selectOneSelecting) { //Check if the first select button was active
                    if (firstSelectedDrone != null) {
                        RemoveDroneFromSelectOne();
                    }
                    AddDroneToSelectOne(drone);
                    DisableSelection();
                } else if (selectTwoSelecting) { //Check if the second select button was active
                    if (secondSelectedDrone != null) {
                        RemoveDroneFromSelectTwo();
                    }
                    AddDroneToSelectTwo(drone);
                    DisableSelection();
                }
                drone.GetComponent<DroneImage>().WasUnselected();
            }
        }
    }

    void Awake()
    {
        // Cache the Game component reference
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            gameComponent = mainCamera.GetComponent<Game>();
            if (gameComponent == null)
            {
                Debug.LogError("Game component not found on Main Camera.");
            }
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }
    }

    public void Breed() {

        DisableSelection();

        if (firstSelectedDrone == null || secondSelectedDrone == null) {
            //TODO: Graceful handling for this situation
            Debug.Log("Didn't have first or second drone selected when trying to breed");
            return;
        }

        Drone breedingTarget1 = firstSelectedDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<Drone>();
        Drone breedingTarget2 = secondSelectedDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<Drone>();
        gameComponent.Breed(breedingTarget1, breedingTarget2);
    }

    public void SelectOne() {
        //First change other select button to not be selecting
        selectTwoSelecting = false;
        //Then set selecting
        selectOneSelecting = true;
    }

    public void SelectTwo() {
        //First change other select button to not be selecting
        selectOneSelecting = false;
        //Then set selecting
        selectTwoSelecting = true;
    }

    public void DisableSelection() {
        selectOneSelecting = false;
        selectTwoSelecting = false;
    }

    void SetupDroneScrollView(List<GameObject> livingAllies) {
        for (var i = 0; i < livingAllies.Count; i++) {
            AddDroneToScrollView(livingAllies[i]);
        }
    }

    void AddDroneToScrollView(GameObject allyDrone) {
        GameObject drone = Instantiate(droneImage);
        drone.transform.SetParent(scrollMenuContent.transform, false);
        drone.GetComponent<DroneImage>().AddLinkedDrone(allyDrone);
        allyDroneList.Add(drone);
    }

    void AddDroneToSelectOne(GameObject allyDrone) {
        firstSelectedDrone = allyDrone;
        allyDrone.transform.SetParent(selectOneContent.transform, false);
    }

    void AddDroneToSelectTwo(GameObject allyDrone) {
        secondSelectedDrone = allyDrone;
        allyDrone.transform.SetParent(selectTwoContent.transform, false);
    }

    void RemoveDroneFromSelectOne() {
        GameObject droneToRemove = firstSelectedDrone;
        droneToRemove.transform.SetParent(scrollMenuContent.transform, false);
        firstSelectedDrone = null;
    }

    void RemoveDroneFromSelectTwo() {
        GameObject droneToRemove = secondSelectedDrone;
        droneToRemove.transform.SetParent(scrollMenuContent.transform, false);
        secondSelectedDrone = null;
    }
}
