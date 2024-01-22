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

    public GameObject pauseMenu;
    public GameObject breedingMenu;
    public GameObject traversalMenu;
    public GameObject gameOverMenu;

    public GameObject[] worldTiles;
    private GameObject leftTile;
    private GameObject middleTile;
    private GameObject rightTile;

    // Start is called before the first frame update
    void Start()
    {
        //spawn in 5 starter drones with default traits
        Trait[] defaultTraits = {

        };
        
        SpawnAlly(new Vector2(0, -1), defaultTraits);
        SpawnAlly(new Vector2(2, 0), defaultTraits);
        SpawnAlly(new Vector2(-2, 0), defaultTraits);
        SpawnAlly(new Vector2(4, 1), defaultTraits);
        SpawnAlly(new Vector2(-4, 1), defaultTraits);

        OpenTraversalMenu();
    }

    // Update is called once per frame
    void Update()
    {
        //maybe check if other menus are already open?
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
    }

    public void StartCombat() {
        //TODO: come up with some logic on where we spawn enemies and what traits we will spawn them with
    }

    public void EndCombat() {
        //set to night
        GetComponent<DayNightCycleManager>().SetNight();
        OpenBreedingMenu();
    }

    void SpawnEnemy(Vector2 pos, Trait[] traits) {
        GameObject enemy = SpawnDrone(enemyDroneObject, pos, traits);
        livingEnemies.Add(enemy);
    }

    void SpawnAlly(Vector2 pos, Trait[] traits) {
        GameObject ally = SpawnDrone(allyDroneObject, pos, traits);
        livingAllies.Add(ally);
    }

    GameObject SpawnDrone(GameObject g, Vector2 pos, Trait[] traits) {
        
        GameObject drone = Instantiate(g);
        drone.transform.position = pos;
        drone.GetComponent<Drone>().AddTraits(traits);
        
        return drone;
    }

    public void TogglePauseMenu() {
        //TODO: make pause menu with settings, quit, restart, etc
        if (!pauseMenu.activeSelf) pauseMenu.SetActive(true);
        else pauseMenu.SetActive(false);
    }
    public void OpenBreedingMenu()
    {
        breedingMenu.SetActive(true);
    }

    public void CloseBreedingMenu()
    {
        breedingMenu.SetActive(false);
        GetComponent<DayNightCycleManager>().SetDay();
        OpenTraversalMenu();
    }

    public void OpenTraversalMenu() {
        //TODO: traversal menu for choosing directions of travel with descriptions both specific and vague on what will be found there

        leftTile = worldTiles[Random.Range(0, worldTiles.Length)];
        middleTile = worldTiles[Random.Range(0, worldTiles.Length)];
        rightTile = worldTiles[Random.Range(0, worldTiles.Length)];

        traversalMenu.SetActive(true);
    }
    public void CloseTraversalMenu() {
        //TODO: traversal menu for choosing directions of travel with descriptions both specific and vague on what will be found there
        traversalMenu.SetActive(false);
    }

    public void TraversalChooseDirection(int choice) {

        switch (choice) {
            case 1:
            //travel diaganol left
            TransitionCamera(leftTile);
            break;

            case 2:
            TransitionCamera(middleTile);
            //travel straight
            break;

            case 3:
            TransitionCamera(rightTile);
            //travel diaganol right
            break;
        }

        //add wait time before combat begins?
        StartCombat();
    }
    void TransitionCamera (GameObject tile) {
        //add some sort of logic to zoom out camera and zoom in out tile.transform.position
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
