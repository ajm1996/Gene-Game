using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // default values subject to change
    public int maxHealth = 10;
    public int currentHealth = 5;
    public int damage = 2;
    public int speed = 1;

    private List<Trait> traits = new List<Trait>();

    void Start()
    {
        // Test: Add a default trait
        AddTrait(new TestTrait());
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

    private void Die()
    {
        Destroy(gameObject);
    }
}