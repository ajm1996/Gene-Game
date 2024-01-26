using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraversalMenu : MonoBehaviour
{

    private Game g;
    public TextMeshProUGUI foodRewardLabel1;
    public TextMeshProUGUI foodRewardLabel2;
    public TextMeshProUGUI foodRewardLabel3;
    public int foodRewardMax = 20;
    public int foodRewardMin = 5;
    private int[] foodRewards = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {
        //g = Camera.main.GetComponent<Game>();
        RandomizeFoodRewards();
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
}
