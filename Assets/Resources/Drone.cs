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

    private Game game;
    private List<Trait> traits = new List<Trait>();
    private Vector2 moveTarget;

    [SerializeField] Healthbar healthbar;

    void Start()
    {
        // Test: Add a default trait
        AddTrait(new TestTrait());

        //initialize healthbar and update it to full
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.UpdateHealthbar(currentHealth, maxHealth);

        game = Camera.main.GetComponent<Game>();
    }

    void Update()
    {
        // Update sprite color based on the last trait
        if (traits.Count > 0)
        {
            GetComponent<SpriteRenderer>().color = traits[traits.Count - 1].Color;
        }


        //if enemies exist, set the moveTarget with MoveTo
        if (game.livingEnemies.Count > 0) {
            
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject g in game.livingEnemies) {

                if (closestEnemy == null) {
                    closestEnemy = g;
                    closestDistance = Vector2.Distance(g.transform.position, transform.position);
                    continue;
                }

                float currentDsitance =  Vector2.Distance(g.transform.position, transform.position);
                if (closestDistance > currentDsitance) {
                    closestEnemy = g;
                    closestDistance = currentDsitance;
                }
            }

            moveTo(closestEnemy);
        }
    }

     void FixedUpdate() {

        if(moveTarget != null) {
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
        }
    }

    public void moveTo(Vector2 target) {
        moveTarget = target;
    }

    public void moveTo(GameObject g) {
        moveTarget = g.transform.position;
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