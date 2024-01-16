using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    public Critter defaultCritter;

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
        var playerCritters = GameObject.FindGameObjectsWithTag("Critter");
        var selectedCritters = new List<Critter>();
        for(int i = 0; i < playerCritters.Length; i++)
        {
            if (playerCritters[i].GetComponent<BoxCollider2D>().IsTouching(GetComponent<BoxCollider2D>()))
            {
                selectedCritters.Add(playerCritters[i].GetComponent<Critter>());
            }
        }
        //Still need to enforce no more than two per pool
        if(selectedCritters.Count > 1)    
            BreedCritters(selectedCritters[0].GetComponent<Critter>(), selectedCritters[1].GetComponent<Critter>());
    }

    public void BreedCritters(Critter critter1, Critter critter2)
    {
        List<Trait> traitList = new List<Trait>();
        traitList.AddRange(critter1.Traits);
        traitList.AddRange(critter2.Traits);

        Critter newCritter1 = defaultCritter;
        Critter newCritter2 = defaultCritter;
        Critter newCritter3 = defaultCritter;

        var instantiatedCritter1 = Instantiate(newCritter1);
        var instantiatedCritter2 = Instantiate(newCritter2);
        var instantiatedCritter3 = Instantiate(newCritter3);
        traitList.Sort();
        Debug.Log(traitList.Count);
        for (int i = 0; i < traitList.Count - 1; i++)
        {
            //They both have the trait, 
            if (traitList[i].Id == traitList[i + 1].Id)
            {
                instantiatedCritter1.Traits.Add(traitList[i]);
                instantiatedCritter2.Traits.Add(traitList[i]);
                instantiatedCritter3.Traits.Add(traitList[i]);
            }
            //Only one has the trait, so 50%
            else
            {
                if (Random.value > 0.5)
                    instantiatedCritter1.Traits.Add(traitList[i]);
                if (Random.value > 0.5)
                    instantiatedCritter2.Traits.Add(traitList[i]);
                if (Random.value > 0.5)
                    instantiatedCritter3.Traits.Add(traitList[i]);
            }
        }
        if (traitList.Count % 2 == 1)
        {
            //Check for last trait
            if (Random.value > 0.5)
                instantiatedCritter1.Traits.Add(traitList[traitList.Count - 1]);
            if (Random.value > 0.5)
                instantiatedCritter2.Traits.Add(traitList[traitList.Count - 1]);
            if (Random.value > 0.5)
                instantiatedCritter3.Traits.Add(traitList[traitList.Count - 1]);
        }

        instantiatedCritter1.transform.position = new Vector3(critter1.transform.position.x - 1, critter1.transform.position.y, 0);
        instantiatedCritter2.transform.position = new Vector3(critter1.transform.position.x, critter1.transform.position.y, 0);
        instantiatedCritter3.transform.position = new Vector3(critter1.transform.position.x + 1, critter1.transform.position.y, 0);

        Destroy(critter1.gameObject);
        Destroy(critter2.gameObject);
    }
}
