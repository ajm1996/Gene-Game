using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraversalMenu : MonoBehaviour
{

    private Game g;
    public TextMeshProUGUI foodRewardLabel1;
    public TextMeshProUGUI foodRewardLabel2;
    public TextMeshProUGUI foodRewardLabel3;
    public int foodRewardMax = 80;
    public int foodRewardMin = 10;
    private int[] foodRewards = new int[3];
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
        DisableOutlines();
        travelButton.GetComponent<Button>().interactable = false;
    }

    public void RandomizeTraits()
    {
        float rand = Random.value;
        int numberOfTraits = 1;
        if(rand > 0.5 - (g.day * 0.4)) numberOfTraits++;
        if (rand > 0.2 - (g.day * 0.2)) numberOfTraits++;

        for (int i=0; i < numberOfTraits; i++) {
            g.enemyTraits.Add(g.possibleTraits[Random.Range(0, g.possibleTraits.Length)]);
        }
        g.numberOfEnemies = (int) (3 + (g.day * Random.Range(0, 0.5f)));
    }

    public void RandomizeFoodRewards() {
        int randomFoodReward = 0;
        for (var i = 0; i < 3; i++) {
            randomFoodReward = Random.Range(foodRewardMin, foodRewardMax);
            foodRewards[i] = randomFoodReward;
            Debug.Log("random food reward: " + randomFoodReward);
        }
        foodRewardLabel1.text = foodRewards[0] + " Food";
        foodRewardLabel2.text = foodRewards[1] + " Food";
        foodRewardLabel3.text = foodRewards[2] + " Food";
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
        g.currentFoodReward = foodRewards[destination];
    }

    public void DisableOutlines() {
        outline1.SetActive(false);
        outline2.SetActive(false);
        outline3.SetActive(false);
    }
}
