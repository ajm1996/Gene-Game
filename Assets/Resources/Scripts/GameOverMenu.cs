using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI daysSurvivedLabel;

    public int daysSurvived;

    public void Init(int daysSurvivedCount) {
        daysSurvived = daysSurvivedCount;
        daysSurvivedLabel.text = "Days Surived: " + daysSurvived;
    }
}
