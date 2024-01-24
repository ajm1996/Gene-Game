using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
    public GameObject droneOneTraitScrollMenuContent;
    public GameObject droneTwoTraitScrollMenuContent;
    public GameObject traitText;

    // Start is called before the first frame update
    void Start()
    {
        //Get list of living ally drones and populate scroll view with it
        List<GameObject> droneList = GameObject.Find("Main Camera").GetComponent<Game>().livingAllies;
        SetupDroneScrollView(droneList);

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

    public void Breed() {

        DisableSelection();

        if (firstSelectedDrone == null || secondSelectedDrone == null) {
            //TODO: Graceful handling for this situation
            Debug.Log("Didn't have first or second drone selected when trying to breed");
            return;
        }

        GameObject.Find("Main Camera").GetComponent<Game>().breedingTarget1 = firstSelectedDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<Drone>();
        GameObject.Find("Main Camera").GetComponent<Game>().breedingTarget2 = secondSelectedDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<Drone>();
        GameObject.Find("Main Camera").GetComponent<Game>().Breed();
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
        List<Trait> droneOneTraits = allyDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<AllyDrone>().GetTraits();
        for (int i = 0; i < droneOneTraits.Count; i++)
        {
            var newTraitText = Instantiate(traitText);
            newTraitText.GetComponent<TextMeshProUGUI>().text = droneOneTraits[i].Id.ToString() + " - " + "Short";
            newTraitText.transform.SetParent(droneOneTraitScrollMenuContent.transform, false);
        }
    }

    void AddDroneToSelectTwo(GameObject allyDrone) {
        secondSelectedDrone = allyDrone;
        allyDrone.transform.SetParent(selectTwoContent.transform, false);
        List<Trait> droneTwoTraits = allyDrone.GetComponent<DroneImage>().linkedDrone.GetComponent<AllyDrone>().GetTraits();
        for (int i = 0; i < droneTwoTraits.Count; i++)
        {
            var newTraitText = Instantiate(traitText);
            newTraitText.GetComponent<TextMeshProUGUI>().text = droneTwoTraits[i].Id.ToString() + " - " + "Description Here With Really Long Text Description";
            newTraitText.transform.SetParent(droneTwoTraitScrollMenuContent.transform, false);
        }
    }

    void RemoveDroneFromSelectOne() {
        GameObject droneToRemove = firstSelectedDrone;
        droneToRemove.transform.SetParent(scrollMenuContent.transform, false);
        var droneOneTraitsList = droneOneTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneOneTraitsList.Length; i++)
        {
            Destroy(droneOneTraitsList[i].gameObject);
        }
        firstSelectedDrone = null;
    }

    void RemoveDroneFromSelectTwo() {
        GameObject droneToRemove = secondSelectedDrone;
        droneToRemove.transform.SetParent(scrollMenuContent.transform, false);
        var droneTwoTraitsList = droneTwoTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneTwoTraitsList.Length; i++)
        {
            Destroy(droneTwoTraitsList[i].gameObject);
        }
        secondSelectedDrone = null;
    }
}
