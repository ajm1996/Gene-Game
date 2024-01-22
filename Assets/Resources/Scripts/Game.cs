using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{


    //TODO: enemy prefab is set to drone prefab until a default enemy prefab is made
    public GameObject enemyDroneObject;
    public GameObject allyDroneObject;
    public List<GameObject> livingEnemies;
    public List<GameObject> livingAllies;

    //Using int counters for count so we don't have to use loops to calculate it
    public int livingEnemiesCount;
    public int livingAlliesCount; 

    public GameObject breedingMenu;

    // Start is called before the first frame update
    void Start()
    {
        //Menu initialization
        breedingMenu = createBreedingMenu();
        //temp spawning for testing
        livingAlliesCount = 0;
        livingEnemiesCount = 0;
        SpawnEnemy(new Vector2(2, 2));
        SpawnAlly(new Vector2(-4, -4));
    }

    // Update is called once per frame
    void Update()
    {
        if (livingAlliesCount <= 0) {
            //TODO: Implement loss functionality
        } else if (livingEnemiesCount <= 0) {
            breedingMenu.SetActive = true;
        }
    }

    GameObject createBreedingMenu() {
        GameObject menu = Instantiate(breedingMenu);
        return menu;
    }

    void SpawnEnemy(Vector2 pos) {
        GameObject enemy = spawnDrone(enemyDroneObject, pos);
        livingEnemies.Add(enemy);
        livingEnemiesCount++;
    }

    void SpawnAlly(Vector2 pos) {
        GameObject ally = spawnDrone(allyDroneObject, pos);
        livingAllies.Add(ally);
        livingAlliesCount++;
    }

    GameObject spawnDrone(GameObject g, Vector2 pos) {
        GameObject drone = Instantiate(g);
        drone.transform.position = pos;
        //TODO: add traits and stats
        return drone;
    }
}