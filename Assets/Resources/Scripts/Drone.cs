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
    public Vector2 moveTarget;
    public List<Vector2> moveQueue;
    private bool moving;
    private float timeOfLastAttack = Mathf.NegativeInfinity;
    
    [HideInInspector]
    public bool  BreedingDeath = false;


    [SerializeField] private Healthbar healthbar;

    public virtual void Start()
    {
        // Test: Add a default trait
        AddTrait(new ExtraFatTrait());

        //initialize healthbar and update it to full
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.UpdateHealthbar(currentHealth, maxHealth);
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
                MoveTo(closestEnemy.gameObject);
            }
        }
    }

     void FixedUpdate() {
        if (moveQueue.Count != 0) moveTarget = moveQueue[0];    //move queue will always override move commands made with MoveTo
        //move towards target
        if(moving) {
            if (Vector2.Distance(transform.position, moveTarget) < 0.05f) { //change value for more or less moving percision
                
                if (moveQueue.Count != 0) moveQueue.RemoveAt(0);
                if (moveQueue.Count != 0) {
                    moveTarget = moveQueue[0]; 
                }
                else {
                    moving = false;
                }

            } 

            if (moving) {
                transform.position = Vector2.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
            }
        }
    }

    public void MoveTo(Vector2 target) {
        moving = true;
        moveTarget = target;
    }

    public void MoveTo(GameObject g) {
        moving = true;
        moveTarget = g.transform.position;
    }

    public void MoveToQueue(Vector2 target) {
        moving = true;
        moveQueue.Add(target);
    }

    public void Attack(Drone enemy) {

        if (Time.time - timeOfLastAttack >= AttackSpeed) {
            enemy.DealDamageCombat(damage);
            DealDamageCombat(enemy.thorns);
            HealDamage(lifesteal);
            timeOfLastAttack = Time.time;
            FindObjectOfType<AudioManager>().Play("DamageDealt");
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