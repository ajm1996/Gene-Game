using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class TraversalMenu : MonoBehaviour
{

    private Game g;
    public TextMeshProUGUI[] foodRewardAndEnemyCountLabels = new TextMeshProUGUI[3];
    public int foodRewardMax = 80;
    public int foodRewardMin = 10;
    private int[] foodRewards = new int[3];
    private int[] enemyCounts = new int[3];
    private List<Trait>[] traitLists = new List<Trait>[3];
    public GameObject traitTextObject;
    public RectTransform[] traitPanels;
    public GameObject travelButton;
    public int destination;
    public GameObject outline1;
    public GameObject outline2;
    public GameObject outline3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {
        g = Camera.main.GetComponent<Game>();

        RandomizeFoodRewards();
        RandomizeTraits();
        RandomizeEnemies();

        GenerateEnemyPreview();
        DisableOutlines();
        travelButton.GetComponent<Button>().interactable = false;
    }


    public void RandomizeEnemies() {

        for (int i=0; i < 3; i++) enemyCounts[i] = (int) (3 + (g.day * Random.Range(0, 0.5f)));
        
    }

    public void RandomizeTraits()
    {
        for (int i=0; i < 3; i++) {
            
            float rand = Random.value;
            int numberOfTraits = 1;

            if (rand < 0.3f - (g.day * 0.04f)) numberOfTraits++;    //30% chance, increase 4% per day
            if (rand < 0.15f + (g.day * 0.02f)) numberOfTraits++;   //15% chance, increase 2% per day

            List<Trait> traits = new List<Trait>();

            for (int j=0; j < numberOfTraits; j++) {
                traits.Add(g.possibleTraits[Random.Range(0, g.possibleTraits.Length)]);
            }
            traitLists[i] = traits;
        }
    }

    public void RandomizeFoodRewards() {
        int randomFoodReward = 0;
        for (var i = 0; i < 3; i++) {
            randomFoodReward = Random.Range(foodRewardMin, foodRewardMax);
            foodRewards[i] = randomFoodReward;
            //Debug.Log("random food reward: " + randomFoodReward);
        }
    }

    private void GenerateEnemyPreview()
    {
        for (int i=0; i < 3; i++) {

             if (Random.value < 0.3)  //30% chance to obfuscate food
                foodRewardAndEnemyCountLabels[i].text = "??";
            else
                foodRewardAndEnemyCountLabels[i].text = "" + foodRewards[i];

            if (Random.value < 0.5)  //50% chance to obfuscate food
                foodRewardAndEnemyCountLabels[i].text += "  Emeies: ?";
            else
                foodRewardAndEnemyCountLabels[i].text += "  Emeies: " + enemyCounts[i];

            foreach (Trait t in traitLists[i]) {

                GameObject enemyTrait = Instantiate(traitTextObject);

                if (Random.value < 0.2)  //20% chance to obfuscate single traits
                    enemyTrait.GetComponent<TextMeshProUGUI>().text = "??? - ???";
                else 
                    enemyTrait.GetComponent<TextMeshProUGUI>().text = t.Name + " - " + t.Description;


                enemyTrait.transform.SetParent(traitPanels[i], false);
            }
        }
    }

    public void SetDestination(int clickedDestination) {
        destination = clickedDestination;
        travelButton.GetComponent<Button>().interactable = true;
        DisableOutlines();
        switch(clickedDestination) {
            case 0:
                outline1.SetActive(true);
                break;
            case 1:
                outline2.SetActive(true);
                break;
            case 2:
                outline3.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Travel() {
        g.TraversalChooseDirection(destination);
        g.enemyTraits = traitLists[destination];
        g.numberOfEnemies = enemyCounts[destination];
        g.currentFoodReward = foodRewards[destination];

        Debug.Log(g.enemyTraits[0].Name);
        Debug.Log(g.currentFoodReward);
        Debug.Log(g.numberOfEnemies);
    }

    public void DisableOutlines() {
        outline1.SetActive(false);
        outline2.SetActive(false);
        outline3.SetActive(false);
    }
}
