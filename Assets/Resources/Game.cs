using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{


    //TODO: enemy prefab is set to drone prefab until a default enemy prefab is made
    public GameObject enemyObject;
    public List<GameObject> livingEnemies;

    // Start is called before the first frame update
    void Start()
    {
        //temp enemy spawn for testing
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy() {
        GameObject enemy = Instantiate(enemyObject);
        //TODO: set enemy traits and stats
        livingEnemies.Add(enemy);
    }
}
