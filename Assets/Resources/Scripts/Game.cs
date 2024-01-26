using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{


    //TODO: enemy prefab is set to drone prefab until a default enemy prefab is made
    public GameObject enemyDroneObject;
    public GameObject allyDroneObject;
    public List<Drone> livingEnemies;
    public List<Drone> livingAllies;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public GameObject[] worldTiles;

    public GameObject breedingMenu;
    public GameObject breedingMenuPrefab;
    public GameObject traversalMenu;
    public GameObject traversalMenuPrefab;

    public int foodCount;
    public int breedingCost;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate and hide menus for later use
        InstantiateMenus();

        livingAllies = new List<Drone>();
        livingEnemies = new List<Drone>();

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
        //show healthbar
        foreach(Drone g in livingAllies) g.GetComponent<AllyDrone>().showHealthbar();

        //TODO: come up with some logic on where we spawn enemies and what traits we will spawn them with
    }

    public void EndCombat() {
        //hide healthbar
        foreach(Drone g in livingAllies) g.GetComponent<AllyDrone>().hideHealthbar();

        //set to night
        GetComponent<DayNightCycleManager>().SetNight();
        OpenBreedingMenu();
    }

    Drone SpawnEnemy(Vector2 pos, List<Trait> traits) {
        Drone enemy = SpawnDrone(enemyDroneObject, pos, traits);
        livingEnemies.Add(enemy);

        //update everyone's attack lists to fight each other
        foreach (Drone ally in livingAllies) {
            ally.attackList.Add(enemy);
            enemy.attackList.Add(ally);
        }

        return enemy;
    }

    public Drone SpawnAlly(Vector2 pos, List<Trait> traits) {
        Drone ally = SpawnDrone(allyDroneObject, pos, traits);
        livingAllies.Add(ally);
        return ally;
    }

    Drone SpawnDrone(GameObject g, Vector2 pos, List<Trait> traits) {
        
        Drone drone = Instantiate(g).GetComponent<Drone>();
        drone.transform.position = pos;
        drone.GetComponent<Drone>().AddTraits(traits);
        
        return drone;
    }

    public void InstantiateMenus() {
        breedingMenu = Instantiate(breedingMenuPrefab);
        breedingMenu.transform.position = Camera.main.transform.position;
        breedingMenu.SetActive(false);

        traversalMenu = Instantiate(traversalMenuPrefab);
        traversalMenu.transform.position = Camera.main.transform.position;
        traversalMenu.SetActive(false);
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
        breedingMenu.GetComponent<BreedingMenu>().Init();
        breedingMenu.SetActive(true);
    }

    public void EndBreedingPhase()
    {
        breedingMenu.SetActive(false);
        foodCount = 0;
        GetComponent<DayNightCycleManager>().SetDay();
        OpenTraversalMenu();
        SpawnEnemy(Vector2.zero, new List<Trait>()); //TODO: REMOVE! for testing purposes
    }

    public void OpenTraversalMenu() {
        if (traversalMenu == null) return;

        //make a grid of randomly generate tiles surrounding the current tile
        for (int i=0; i < 3; i++) {
            for (int j=0; j < 3; j++) {
                if (i == 1 && j == 1) continue;
                Instantiate(worldTiles[Random.Range(0, worldTiles.Length)]).transform.position += new Vector3(-24 + 24 * i, -16 + 16 * j);
            }
        }
        
        traversalMenu.transform.position = Camera.main.transform.position;
        traversalMenu.GetComponent<TraversalMenu>().Init();
        traversalMenu.SetActive(true);
    }
    public void CloseTraversalMenu() {
        traversalMenu.SetActive(false);
    }

    public void TraversalChooseDirection(int choice) {

        switch (choice) {
            case 1:
            //travel diaganol left
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(-24, 0, -10));
            break;

            case 2:
            //travel straight
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(0, 16, -10));
            break;

            case 3:
            //travel diaganol right
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(24, 0, -10));
            break;
        }

        //add wait time before combat begins?
        StartCombat();
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
