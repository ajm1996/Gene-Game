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

    void Start()
    {
        // Test: Add a default trait
        AddTrait(new TestTrait());
    }

    public void Initialize(int newHealth = 10, int newDamage = 5, int newSpeed = 5) {
        maxHealth = newHealth;
        currentHealth = newHealth;
        damage = newDamage;
        speed = newSpeed;
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
        if (currentHealth <= 0) Die();
    }

    public void HealDamage(int healAmount)
    {
        currentHealth += healAmount;
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