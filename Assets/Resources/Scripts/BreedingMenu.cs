using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BreedingMenu : MonoBehaviour
{

    public GameObject scrollMenuContent;

    public GameObject droneImage;

    public List<GameObject> allyDroneList;

    public GameObject breedingSelectOneContent;
    public GameObject breedingSelectTwoContent;

    public GameObject inspectorSelectOneContent;
    public DroneImage inspectorSelection;

    public GameObject breedingOneTraitScrollMenuContent;
    public GameObject breedingTwoTraitScrollMenuContent;
    public GameObject inspectorTraitScrollMenuContent;

    public GameObject deselectButton1;
    public GameObject deselectButton2;
    public GameObject inspectorDeselectButton;

    public GameObject traitText;

    private Game g;

    public Drone breedingTarget1;
    public Drone breedingTarget2;

    public GameObject breedingSubMenu;
    public GameObject inspectorSubMenu;


    // Start is called before the first frame update
    void Start()
    {
        g = Camera.main.GetComponent<Game>();
        List<Drone> droneList = g.livingAllies;
        SetupDroneScrollView(droneList);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Breed() {
        if (breedingTarget1 == null || breedingTarget2 == null) return;

        transform.GetChild(0).gameObject.SetActive(false);

        //move them away from the group first? to a dedicated breeding area
        //smooching time
        breedingTarget1.MoveTo(breedingTarget2.transform.position);
        breedingTarget2.MoveTo(breedingTarget1.transform.position);
        //TODO: add heart animation
        //TODO: add wait time for full breeding animation to complete

        //average of the parents positions (for birth positions)
        Vector2 averagePos = (breedingTarget1.transform.position + breedingTarget2.transform.position) / 2f;

        List<Trait> traitList1 = breedingTarget1.GetTraits();
        List<Trait> traitList2 = breedingTarget2.GetTraits();
        List<Trait> sharedList = new List<Trait>();

        //find traits in common and remove them into their own list
        foreach(Trait t in traitList1) {
            if (traitList2.Contains(t)) {
                sharedList.Add(t);
                traitList1.Remove(t);
                traitList2.Remove(t);
            }
        }

        //create 3 baby drones
        for (int i = 0; i < 3; i++) {
            List<Trait> traitList = new List<Trait>();

            //100% chance for common traits
            foreach(Trait t in sharedList) traitList.Add(t); 

            //50% chance for all uncommon traits
            foreach (Trait t in traitList1) {
                if(Random.value > 0.5f) traitList.Add(t);
            }
            foreach (Trait t in traitList2) {
                if(Random.value > 0.5f) traitList.Add(t);
            }

            //set their birthing position and their move target
            Drone newSpawn  = g.SpawnAlly(new Vector2(averagePos.x - 2 + (i * 2), averagePos.y - 1), traitList);
            newSpawn.MoveTo(new Vector2(0,0)); //TODO: move them back to the group after a small wait time
        }

        //kill breeding targets
        breedingTarget1.Die();
        breedingTarget2.Die();

        //destroy image for those breeders
        Destroy(breedingSelectOneContent.transform.GetChild(1).gameObject);
        Destroy(breedingSelectTwoContent.transform.GetChild(1).gameObject);

        //hide deselect buttons
        deselectButton1.SetActive(false);
        deselectButton2.SetActive(false);

        //destroy all trait text
        var droneOneTraitsList = breedingOneTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneOneTraitsList.Length; i++)
        {
            Destroy(droneOneTraitsList[i].gameObject);
        }

        var droneTwoTraitsList = breedingTwoTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneTwoTraitsList.Length; i++)
        {
            Destroy(droneTwoTraitsList[i].gameObject);
        }

        //reset breeding targets
        breedingTarget1 = null;
        breedingTarget2 = null;

        transform.GetChild(0).gameObject.SetActive(true);

        CloseBreedingSubMenu();
    }


    public void SelectToBreed(DroneImage droneImage) {

        if(breedingSubMenu.activeSelf) { //check which menu state we're in
            //if square 1 is empty, fill it
            if(breedingTarget1 == null) {
                SelectBreedingContent(true, droneImage);
            } 
            else {

                //if square 2 isn't empty, empty it, then fill it.
                if(breedingTarget2 != null) {
                    DeselectBreedingContent(false);
                }

                SelectBreedingContent(false, droneImage);
            }

        } else { //we're in the inspector menu

            if (inspectorSelection != null) { //already populated, wipe it
                DeselectInspectorContent(true);
            }

            Drone d = droneImage.linkedDrone.GetComponent<Drone>();
            inspectorSelection = droneImage;

            //move to the selection square and reset position to zero
            droneImage.transform.SetParent(inspectorSelectOneContent.transform, false);
            droneImage.transform.localPosition = Vector2.zero;

            //add traits as text to the scroll list below the selection square
            List<Trait> droneOneTraits = d.GetComponent<AllyDrone>().GetTraits();
            for (int i = 0; i < droneOneTraits.Count; i++)
            {
                var newTraitText = Instantiate(traitText);
                newTraitText.GetComponent<TextMeshProUGUI>().text = droneOneTraits[i].Id.ToString() + " - " + "Short";
                newTraitText.transform.SetParent(inspectorTraitScrollMenuContent.transform, false);
            }
            //show deselect button
            inspectorDeselectButton.SetActive(true);
        }
    }

    public void SelectBreedingContent(bool isOne, DroneImage droneImage) {
        Drone d = droneImage.linkedDrone.GetComponent<Drone>();

        //check whether target square is 1 or 2
        GameObject target = isOne ? breedingSelectOneContent : breedingSelectTwoContent;

        //move to the selection square and reset position to zero
        droneImage.transform.SetParent(target.transform, false);
        droneImage.transform.localPosition = Vector2.zero;

        //add traits as text to the scroll list below the selection square
        List<Trait> droneOneTraits = d.GetComponent<AllyDrone>().GetTraits();
        for (int i = 0; i < droneOneTraits.Count; i++)
        {
            var newTraitText = Instantiate(traitText);
            newTraitText.GetComponent<TextMeshProUGUI>().text = droneOneTraits[i].Id.ToString() + " - " + "Short";
            
            if (isOne) newTraitText.transform.SetParent(breedingOneTraitScrollMenuContent.transform, false);
            else newTraitText.transform.SetParent(breedingTwoTraitScrollMenuContent.transform, false);
        }

        //set proper breeding target
        if (isOne)  breedingTarget1 = d;
        else breedingTarget2 = d;

        //show deselect buttons
        if (isOne) deselectButton1.SetActive(true);
        else deselectButton2.SetActive(true);
    }

    public void DeselectBreedingContent(bool isOne) {
        //check whether target square is 1 or 2
        GameObject target = isOne ? breedingSelectOneContent : breedingSelectTwoContent;

        //send back to scroll menu
        if (target.transform.childCount > 1)
            target.transform.GetChild(1).SetParent(scrollMenuContent.transform, false);

        //delete appropriate trait list
        var targetTraitsList = isOne ? 
        breedingOneTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>() :
        breedingTwoTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < targetTraitsList.Length; i++)
        {
            Destroy(targetTraitsList[i].gameObject);
        }

        //reset breeding target values
        if (isOne) breedingTarget1 = null;
        else breedingTarget2 = null;

        //hide deselect buttons
        if (isOne) deselectButton1.SetActive(false);
        else deselectButton2.SetActive(false);
    }

    public void DeselectInspectorContent(bool shouldMove) {
        //move selection back to drone list
        if (shouldMove) inspectorSelection.transform.SetParent(scrollMenuContent.transform, false);

        //wipe selection
        inspectorSelection = null;

        //clean traits list
        var targetTraitsList = inspectorTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < targetTraitsList.Length; i++)
        {
            Destroy(targetTraitsList[i].gameObject);
        }

        //hide deselect button
        inspectorDeselectButton.SetActive(false);
    }


    void SetupDroneScrollView(List<Drone> livingAllies) {
        for (var i = 0; i < livingAllies.Count; i++) {
            AddDroneToScrollView(livingAllies[i]);
        }
    }

    void AddDroneToScrollView(Drone allyDrone) {
        GameObject drone = Instantiate(droneImage);
        drone.transform.SetParent(scrollMenuContent.transform, false);
        drone.GetComponent<DroneImage>().AddLinkedDrone(allyDrone);
        allyDroneList.Add(drone);
    }

    public void EndBreedingPhase() {
        g.EndBreedingPhase();
    }

    public void OpenBreedingSubMenu() {
        inspectorSubMenu.SetActive(false);
        breedingSubMenu.SetActive(true);

        if (inspectorSelection != null) {
            SelectBreedingContent(true, inspectorSelection);
            DeselectInspectorContent(false);
        }
    }

    public void CloseBreedingSubMenu() {
        //wipe selection and return to drone list
        DeselectBreedingContent(true);
        DeselectBreedingContent(false);

        inspectorSubMenu.SetActive(true);
        breedingSubMenu.SetActive(false);
    }
}
