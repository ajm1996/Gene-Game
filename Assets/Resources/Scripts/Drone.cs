using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drone : MonoBehaviour
{
    // default values subject to change
    [SerializeField] public  int maxHealth;
    [SerializeField] public  int currentHealth;
    [SerializeField] public  int damage;
    [SerializeField] public  float speed;
    [SerializeField] public  float AttackSpeed; //in seconds
    [SerializeField] public  float AttackDistance;
    [SerializeField] public  Color color;

    public List<GameObject> attackList;
    private List<Trait> traits = new List<Trait>();
    private Vector2 moveTarget;
    private bool moving;
    private float timeOfLastAttack = Mathf.NegativeInfinity;


    [SerializeField] private Healthbar healthbar;

    public virtual void Start()
    {
        // Test: Add a default trait
        AddTrait(new TestTrait());

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
            
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            //find closest target in attacklist
            foreach (GameObject g in attackList) {

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

    public void MoveTo(GameObject g) {
        moving = true;
        moveTarget = g.transform.position;
    }

    public void Attack(GameObject enemy) {

        if (Time.time - timeOfLastAttack >= AttackSpeed) {
            enemy.GetComponent<Drone>().DealDamage(damage);
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

    public void DealDamage(int damageAmount)
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
        Game g = Camera.main.GetComponent<Game>();
        g.livingAllies.Remove(gameObject);
        g.livingEnemies.Remove(gameObject);

        if (g.livingAllies.Count == 0) g.GameOver();
        if (g.livingEnemies.Count == 0) g.EndCombat();

        Destroy(gameObject);
    }

    public void showHealthbar() {
        healthbar.gameObject.SetActive(true);
    }

    public void hideHealthbar() {
        healthbar.gameObject.SetActive(false);
    }
}