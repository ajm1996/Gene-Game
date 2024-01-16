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
            var spawningPools = GameObject.FindGameObjectsWithTag("SpawningPool");
;           for(int i = 0; i < spawningPools.Length; i++)
            {
                spawningPools[i].GetComponent<SpawningPool>().BreedTime();
            }
            DayTimeLength = TIMECYCLELENGTH;
        }
    }
}
