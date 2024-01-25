using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreedingMenu : MonoBehaviour
{

    public List<DroneImage> droneImages= new List<DroneImage>();
    public GameObject scrollMenuContent;

    public GameObject droneImage;

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

    public DroneImage breedingTarget1;
    public DroneImage breedingTarget2;

    public GameObject breedingSubMenu;
    public GameObject inspectorSubMenu;
    public Button breedingButton;


    // Start is called before the first frame update
    public void Start()
    {

    }

    public void Init() {
        g = Camera.main.GetComponent<Game>();
        List<Drone> droneList = g.livingAllies;
        SetupDroneScrollView(droneList);
        breedingButton.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Breed() {
        if (breedingTarget1 == null || breedingTarget2 == null) return;
        if (g.foodCount < g.breedingCost) {
            Debug.Log("breeding method entered without enough food!");
            return;
        }
            

        transform.GetChild(0).gameObject.SetActive(false);

        //move them away from the group first? to a dedicated breeding area
        //smooching time
        breedingTarget1.linkedDrone.MoveTo(breedingTarget2.transform.position);
        breedingTarget2.linkedDrone.MoveTo(breedingTarget1.transform.position);
        //TODO: add heart animation
        //TODO: add wait time for full breeding animation to complete

        //average of the parents positions (for birth positions)
        Vector2 averagePos = (breedingTarget1.transform.position + breedingTarget2.transform.position) / 2f;

        List<Trait> traitList1 = breedingTarget1.linkedDrone.GetTraits();
        List<Trait> traitList2 = breedingTarget2.linkedDrone.GetTraits();
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
            DroneImage di = AddDroneToScrollView(newSpawn);
            di.isChild = true;
        }

        //kill breeding targets
        breedingTarget1.linkedDrone.Die();
        breedingTarget2.linkedDrone.Die();

        //destroy image for those breeders
        Destroy(breedingTarget1.gameObject);
        Destroy(breedingTarget2.gameObject);

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

        transform.GetChild(0).gameObject.SetActive(true);
        CloseBreedingSubMenu();
        
    }

    public void SelectDrone(DroneImage droneImage) {

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
            SelectInspectorContent(droneImage);
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
        if (isOne)  breedingTarget1 = droneImage;
        else breedingTarget2 = droneImage;

        //show deselect buttons
        if (isOne) deselectButton1.SetActive(true);
        else deselectButton2.SetActive(true);
    }
    public void DeselectBreedingContent(bool isOne) {
        //check whether target square is 1 or 2
        GameObject target = isOne ? breedingSelectOneContent : breedingSelectTwoContent;
        
        if(isOne) breedingTarget1.transform.SetParent(scrollMenuContent.transform, false);
        else breedingTarget2.transform.SetParent(scrollMenuContent.transform, false);

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

        //make button interactable if both targets aren't children
        if ((breedingTarget1 == null || !breedingTarget1.isChild) && (breedingTarget2 == null || !breedingTarget2.isChild)) {
            breedingButton.interactable = true;
        }

        //hide deselect buttons
        if (isOne) deselectButton1.SetActive(false);
        else deselectButton2.SetActive(false);
    }

    public void SelectInspectorContent(DroneImage droneImage) {
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

    DroneImage AddDroneToScrollView(Drone allyDrone) {
        DroneImage drone = Instantiate(droneImage).GetComponent<DroneImage>();
        drone.transform.SetParent(scrollMenuContent.transform, false);
        drone.GetComponent<DroneImage>().AddLinkedDrone(allyDrone);
        droneImages.Add(drone);
        return drone;
    }

    public void OpenBreedingSubMenu() {

        if (g.foodCount < g.breedingCost) breedingButton.interactable = false;

        foreach(DroneImage di in droneImages) {
            
            if (di.isChild) {
                di.GetComponent<Button>().interactable = false;
                Debug.Log("disable interactable");
            }
        }

        if (inspectorSelection != null) {
            SelectBreedingContent(true, inspectorSelection);
            DeselectInspectorContent(false);
        }
        
        inspectorSubMenu.SetActive(false);
        breedingSubMenu.SetActive(true);
    }

    public void CloseBreedingSubMenu() {
        //wipe selection and return to drone list

        if (breedingTarget1 != null) DeselectBreedingContent(true);
        if (breedingTarget2 != null) DeselectBreedingContent(false);

        foreach(DroneImage di in droneImages) {
            if (di.isChild) {
                di.GetComponent<Button>().interactable = true;
                Debug.Log("make interactable");
            }
        }

        inspectorSubMenu.SetActive(true);
        breedingSubMenu.SetActive(false);
        
    }

    public void EndBreedingPhase() {
        foreach(DroneImage di in droneImages) {
            Destroy(di.gameObject);
        }
        droneImages = new List<DroneImage>();
        g.EndBreedingPhase();
    }
}
