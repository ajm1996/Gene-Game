using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drone : MonoBehaviour
{
    // default values subject to change
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public int speed;
    public Color color;

    private List<Trait> traits = new List<Trait>();

    [SerializeField] Healthbar healthbar;

    void Start()
    {
        // Test: Add a default trait
        AddTrait(new TestTrait());

        //initialize healthbar and update it to full
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
    }

    void Update()
    {
        // Update sprite color based on the last trait
        if (traits.Count > 0)
        {
            GetComponent<SpriteRenderer>().color = traits[traits.Count - 1].Color;
        }
    }

    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        trait.ApplyEffect(this);
        // calculateColor();
    }

    public void RemoveTrait(Trait trait)
    {
        traits.Remove(trait);
    }

    public List<Trait> GetTraits()
    {
        return traits;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    public void HealDamage(int healAmount)
    {
        currentHealth += healAmount;
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    
    // TODO: calculate drone color
    // private void calculateColor()
    // {
    //     Color lastTraitColor = traits[traits.Count - 1].Color;

    //     color = (color + lastTraitColor) / 2;
        
    // }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}