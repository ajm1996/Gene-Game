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
    public GameObject selectTwoContent;
    public GameObject droneOneTraitScrollMenuContent;
    public GameObject droneTwoTraitScrollMenuContent;
    public GameObject traitText;

    private Game g;
    public Drone breedingTarget1;
    public Drone breedingTarget2;

    // Start is called before the first frame update
    void Start()
    {
        g = Camera.main.GetComponent<Game>();
        List<GameObject> droneList = g.livingAllies;
        SetupDroneScrollView(droneList);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Breed() {
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
            GameObject newSpawn  = g.SpawnAlly(new Vector2(averagePos.x - 2 + (i * 2), averagePos.y - 1), traitList);
            newSpawn.GetComponent<Drone>().MoveTo(new Vector2(0,0)); //TODO: move them back to the group after a small wait time
        }

        //kill breeding targets
        breedingTarget1.Die();
        breedingTarget2.Die();

        //destroy buttons for those breeders
        Destroy(selectOneContent.transform.GetChild(0).gameObject);
        Destroy(selectTwoContent.transform.GetChild(0).gameObject);

        //destroy all trait text
        var droneOneTraitsList = droneOneTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneOneTraitsList.Length; i++)
        {
            Destroy(droneOneTraitsList[i].gameObject);
        }

        var droneTwoTraitsList = droneTwoTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < droneTwoTraitsList.Length; i++)
        {
            Destroy(droneTwoTraitsList[i].gameObject);
        }

        //reset breeding targets
        breedingTarget1 = null;
        breedingTarget2 = null;

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SelectToBreed(DroneImage droneImage) {

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
    }

    public void SelectBreedingContent(bool isOne, DroneImage droneImage) {
        Drone d = droneImage.linkedDrone.GetComponent<Drone>();

        //check whether target square is 1 or 2
        GameObject target = isOne ? selectOneContent : selectTwoContent;

        //move to the selection square and reset position to zero
        droneImage.transform.SetParent(target.transform, false);
        droneImage.transform.localPosition = Vector2.zero;

        //add traits as text to the scroll list below the selection square
        List<Trait> droneOneTraits = d.GetComponent<AllyDrone>().GetTraits();
        for (int i = 0; i < droneOneTraits.Count; i++)
        {
            var newTraitText = Instantiate(traitText);
            newTraitText.GetComponent<TextMeshProUGUI>().text = droneOneTraits[i].Id.ToString() + " - " + "Short";
            
            if (isOne) newTraitText.transform.SetParent(droneOneTraitScrollMenuContent.transform, false);
            else newTraitText.transform.SetParent(droneTwoTraitScrollMenuContent.transform, false);
        }

        //set proper breeding target
        if (isOne)  breedingTarget1 = d;
        else breedingTarget2 = d;
    }

    public void DeselectBreedingContent(bool isOne) {
        //check whether target square is 1 or 2
        GameObject target = isOne ? selectOneContent : selectTwoContent;

        //send back to scroll menu
        target.transform.GetChild(0).SetParent(scrollMenuContent.transform, false);

        //delete appropriate trait list
        var targetTraitsList = isOne ? 
        droneOneTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>() :
        droneTwoTraitScrollMenuContent.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < targetTraitsList.Length; i++)
        {
            Destroy(targetTraitsList[i].gameObject);
        }

        //reset breeding target values
        if (isOne) breedingTarget1 = null;
        else breedingTarget2 = null;
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
}
