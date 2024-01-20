using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DayNightCycle : MonoBehaviour
{
    private float TIMECYCLELENGTH = 3f;
    public float DayTimeLength = 3f;
    public Drone defaultDrone;

    // Start is called before the first frame update
    void Start()
    {
        //Simply assigning colors to the 4 starting Drones at the start
        var playerDrones = GameObject.FindGameObjectsWithTag("Drone");
        for(int i = 0; i < playerDrones.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 2) < 1)
            {
                playerDrones[i].GetComponent<Drone>().AddTrait(new TestTrait());
            }
            else
            {
                playerDrones[i].GetComponent<Drone>().AddTrait(new TestTrait());
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
