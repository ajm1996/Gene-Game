using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    public Drone defaultDrone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreedTime()
    {
        var playerDrones = GameObject.FindGameObjectsWithTag("Drone");
        var selectedDrones = new List<Drone>();
        for(int i = 0; i < playerDrones.Length; i++)
        {
            if (playerDrones[i].GetComponent<BoxCollider2D>().IsTouching(GetComponent<BoxCollider2D>()))
            {
                selectedDrones.Add(playerDrones[i].GetComponent<Drone>());
            }
        }
        //Still need to enforce no more than two per pool
        if(selectedDrones.Count > 1)    
            BreedDrones(selectedDrones[0].GetComponent<Drone>(), selectedDrones[1].GetComponent<Drone>());
    }

    public void BreedDrones(Drone Drone1, Drone Drone2)
    {
        List<Trait> traitList = new List<Trait>();
        traitList.AddRange(Drone1.GetTraits());
        traitList.AddRange(Drone2.GetTraits());

        Drone newDrone1 = defaultDrone;
        Drone newDrone2 = defaultDrone;
        Drone newDrone3 = defaultDrone;

        var instantiatedDrone1 = Instantiate(newDrone1);
        var instantiatedDrone2 = Instantiate(newDrone2);
        var instantiatedDrone3 = Instantiate(newDrone3);
        traitList.Sort();
        Debug.Log(traitList.Count);
        for (int i = 0; i < traitList.Count - 1; i++)
        {
            //They both have the trait, 
            if (traitList[i].Id == traitList[i + 1].Id)
            {
                instantiatedDrone1.AddTrait(traitList[i]);
                instantiatedDrone2.AddTrait(traitList[i]);
                instantiatedDrone3.AddTrait(traitList[i]);
            }
            //Only one has the trait, so 50%
            else
            {
                if (Random.value > 0.5)
                    instantiatedDrone1.AddTrait(traitList[i]);
                if (Random.value > 0.5)
                    instantiatedDrone2.AddTrait(traitList[i]);
                if (Random.value > 0.5)
                    instantiatedDrone3.AddTrait(traitList[i]);
            }
        }
        if (traitList.Count % 2 == 1)
        {
            //Check for last trait
            if (Random.value > 0.5)
                instantiatedDrone1.AddTrait(traitList[traitList.Count - 1]);
            if (Random.value > 0.5)
                instantiatedDrone2.AddTrait(traitList[traitList.Count - 1]);
            if (Random.value > 0.5)
                instantiatedDrone3.AddTrait(traitList[traitList.Count - 1]);
        }

        instantiatedDrone1.transform.position = new Vector3(Drone1.transform.position.x - 1, Drone1.transform.position.y, 0);
        instantiatedDrone2.transform.position = new Vector3(Drone1.transform.position.x, Drone1.transform.position.y, 0);
        instantiatedDrone3.transform.position = new Vector3(Drone1.transform.position.x + 1, Drone1.transform.position.y, 0);

        Destroy(Drone1.gameObject);
        Destroy(Drone2.gameObject);
    }
}
