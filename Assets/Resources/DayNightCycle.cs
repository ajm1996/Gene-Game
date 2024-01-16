using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DayNightCycle : MonoBehaviour
{
    private float TIMECYCLELENGTH = 3f;
    public float DayTimeLength = 3f;
    public Critter defaultCritter;

    // Start is called before the first frame update
    void Start()
    {
        //Simply assigning colors to the 4 starting critters at the start
        Critter tempCritter = new Critter();
        var playerCritters = GameObject.FindGameObjectsWithTag("Critter");
        for(int i = 0; i < playerCritters.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 2) < 1)
            {
                playerCritters[i].GetComponent<Critter>().Traits.Add(new Trait() { Id = 1, Color = Color.red });
            }
            else
            {
                playerCritters[i].GetComponent<Critter>().Traits.Add(new Trait() { Id = 2, Color = Color.yellow });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DayTimeLength > 0f)
        {
            DayTimeLength -= Time.deltaTime;
        }
        else
        {
            Critter tempCritter = new Critter();
            var playerCritters = GameObject.FindGameObjectsWithTag("Critter");
            for(int i = 0; i < playerCritters.Length; i++)
            {
                if(i % 2 == 0)
                {
                    tempCritter = playerCritters[i].GetComponent<Critter>();
                }
                else
                {
                    //Breeds critters every 3 seconds
                    BreedCritters(tempCritter, playerCritters[i].GetComponent<Critter>());
                }
            }
            DayTimeLength = TIMECYCLELENGTH;
        }
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
                if (Random.Range(0, 2) < 1)
                    instantiatedCritter1.Traits.Add(traitList[i]);
                if (Random.Range(0, 2) < 1)
                    instantiatedCritter2.Traits.Add(traitList[i]);
                if (Random.Range(0, 2) < 1)
                    instantiatedCritter3.Traits.Add(traitList[i]);
            }
        }
        if(traitList.Count % 2 == 1)
        {
            //Check for last trait
            if (Random.Range(0, 2) < 1)
                instantiatedCritter1.Traits.Add(traitList[traitList.Count - 1]);
            if (Random.Range(0, 2) < 1)
                instantiatedCritter2.Traits.Add(traitList[traitList.Count - 1]);
            if (Random.Range(0, 2) < 1)
                instantiatedCritter3.Traits.Add(traitList[traitList.Count - 1]);
        }

        instantiatedCritter1.transform.position = new Vector3(critter1.transform.position.x - 1, critter1.transform.position.y, 0);
        instantiatedCritter2.transform.position = new Vector3(critter1.transform.position.x, critter1.transform.position.y, 0);
        instantiatedCritter3.transform.position = new Vector3(critter1.transform.position.x + 1, critter1.transform.position.y, 0);

        Destroy(critter1.gameObject);
        Destroy(critter2.gameObject);
    }
}
