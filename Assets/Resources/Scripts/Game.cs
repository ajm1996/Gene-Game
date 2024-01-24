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
    public GameObject traversalMenu;
    public GameObject gameOverMenu;

    public GameObject[] worldTiles;
    private GameObject leftTile;
    private GameObject middleTile;
    private GameObject rightTile;

    public GameObject breedingMenu;
    public GameObject breedingMenuPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate and hide menus for later use
        InstantiateMenus();

        //spawn in 5 starter drones with default traits
        List<Trait> defaultTraits = new List<Trait>();
        
        SpawnAlly(new Vector2(0, -1), defaultTraits);
        SpawnAlly(new Vector2(2, 0), defaultTraits);
        SpawnAlly(new Vector2(-2, 0), defaultTraits);
        SpawnAlly(new Vector2(4, 1), defaultTraits);
        SpawnAlly(new Vector2(-4, 1), defaultTraits);
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());
        livingAllies[0].GetComponent<Drone>().AddTrait(new TestTrait2());

        //TODO: Change this back to OpenTraversalMenu when breeding menu is ready
        OpenBreedingMenu();
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
        //show healthbar
        foreach(GameObject g in livingAllies) g.GetComponent<AllyDrone>().showHealthbar();

        //TODO: come up with some logic on where we spawn enemies and what traits we will spawn them with
    }

    public void EndCombat() {
        //hide healthbar
        foreach(GameObject g in livingAllies) g.GetComponent<AllyDrone>().hideHealthbar();

        //set to night
        GetComponent<DayNightCycleManager>().SetNight();
        OpenBreedingMenu();
    }

    void SpawnEnemy(Vector2 pos, List<Trait> traits) {
        GameObject enemy = SpawnDrone(enemyDroneObject, pos, traits);
        livingEnemies.Add(enemy);
    }

    public GameObject SpawnAlly(Vector2 pos, List<Trait> traits) {
        GameObject ally = SpawnDrone(allyDroneObject, pos, traits);
        livingAllies.Add(ally);
        return ally;
    }

    GameObject SpawnDrone(GameObject g, Vector2 pos, List<Trait> traits) {
        
        GameObject drone = Instantiate(g);
        drone.transform.position = pos;
        drone.GetComponent<Drone>().AddTraits(traits);
        
        return drone;
    }

    public void InstantiateMenus() {
        breedingMenu = Instantiate(breedingMenuPrefab);
        breedingMenu.transform.position = Camera.main.transform.position;
        breedingMenu.SetActive(false);

        //TODO: Instantiate traversal menu
    }

    public void TogglePauseMenu() {
        //TODO: make pause menu with settings, quit, restart, etc
        if (!pauseMenu.activeSelf) pauseMenu.SetActive(true);
        else pauseMenu.SetActive(false);
    }
    public void OpenBreedingMenu()
    {
        Debug.Log("open breeding menu");
        breedingMenu.transform.position = Camera.main.transform.position;
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

        if(worldTiles.Length != 0) {
            leftTile = worldTiles[Random.Range(0, worldTiles.Length)];
            middleTile = worldTiles[Random.Range(0, worldTiles.Length)];
            rightTile = worldTiles[Random.Range(0, worldTiles.Length)];
        }
        else Debug.Log("worldTiles array is empty, no world tiles to choose from");
        

        traversalMenu.SetActive(true);
    }
    public void CloseTraversalMenu() {
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
