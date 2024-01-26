using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Drone : MonoBehaviour
{
    // default values subject to change
    [SerializeField] public  int maxHealth;
    [SerializeField] public  int currentHealth;
    [SerializeField] public  int damage;
    [SerializeField] public  int armor;
    [SerializeField] public  int lifesteal;
    [SerializeField] public  int thorns;
    [SerializeField] public  float speed;
    [SerializeField] public  float AttackSpeed; //in seconds
    [SerializeField] public  float AttackDistance;
    [SerializeField] public  Color color;

    public List<Drone> attackList;
    private List<Trait> traits = new List<Trait>();
    private Vector2 moveTarget;
    private bool moving;
    private float timeOfLastAttack = Mathf.NegativeInfinity;


    [SerializeField] private Healthbar healthbar;

    public virtual void Start()
    {
        // Test: Add a default trait
        AddTrait(new ExtraFatTrait());

        //initialize healthbar and update it to full
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
        
        moveTarget = transform.position;

    }

    public virtual void Update()
    {
        // Update sprite color based on the last trait
        if (traits.Count > 0)
        {
            GetComponent<SpriteRenderer>().color = traits[traits.Count - 1].Color;
        }

        //if enemies exist, set the moveTarget with MoveTo
        if (attackList.Count > 0) {
            
            Drone closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            //find closest target in attacklist
            foreach (Drone d in attackList) {

                if (closestEnemy == null) {
                    closestEnemy = d;
                    closestDistance = Vector2.Distance(d.transform.position, transform.position);
                    continue;
                }

                float currentDistance =  Vector2.Distance(d.transform.position, transform.position);
                if (closestDistance > currentDistance) {
                    closestEnemy = d;
                    closestDistance = currentDistance;
                }
            }

            if (closestDistance <= AttackDistance) {
                moving = false;
                Attack(closestEnemy);
            } else {
                MoveTo(closestEnemy);
            }
        }
    }

     void FixedUpdate() {
        //move towards target
        if(moving) {
            if (Vector2.Distance(transform.position, moveTarget) < 0.01f) { //change value for more or less moving percision
                moving = false;
            } else {
                transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
            }
        }
    }

    public void MoveTo(Vector2 target) {
        moving = true;
        moveTarget = target;
    }

    public void MoveTo(Drone d) {
        moving = true;
        moveTarget = d.transform.position;
    }

    public void Attack(Drone enemy) {

        if (Time.time - timeOfLastAttack >= AttackSpeed) {
            enemy.GetComponent<Drone>().DealDamageCombat(damage);
            DealDamageCombat(enemy.GetComponent<Drone>().thorns);
            HealDamage(lifesteal);
            timeOfLastAttack = Time.time;
        }
    }


    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        trait.ApplyEffect(this);
        // calculateColor();
    }

    public void AddTraits(List<Trait> traits) {
        foreach (Trait trait in traits) AddTrait(trait);
    }

    public void RemoveTrait(Trait trait)
    {
        traits.Remove(trait);
    }

    public List<Trait> GetTraits()
    {
        return traits;
    }

    public void DealDamageCombat(int damageAmount)
    {
        currentHealth -= Math.Max(1, damageAmount - armor);
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
        if (currentHealth <= 0) {
            Die();
            CheckForWinCondition();
        }
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
    
    public abstract void Die();

    public void CheckForWinCondition() {
        Game g = Camera.main.GetComponent<Game>();
        if (g.livingAllies.Count == 0) g.GameOver();
        if (g.livingEnemies.Count == 0) g.EndCombat();
    }

    public void showHealthbar() {
        healthbar.gameObject.SetActive(true);
    }

    public void hideHealthbar() {
        healthbar.gameObject.SetActive(false);
    }
}