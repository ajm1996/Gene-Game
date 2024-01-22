using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthbar(float currentHealth, float maxHealth) {
        slider.value = currentHealth / maxHealth;
    }
}
