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
        SpawnAlly(new Vector2(-4, 1), defaultTraits);
        SpawnAlly(new Vector2(-2, 0), defaultTraits);
        SpawnAlly(new Vector2(0, -1), defaultTraits);
        SpawnAlly(new Vector2(2, 0), defaultTraits);
        SpawnAlly(new Vector2(4, 1), defaultTraits);

        // SpawnAlly(new Vector2(-4, 3), defaultTraits);
        // SpawnAlly(new Vector2(-2, 2), defaultTraits);
        // SpawnAlly(new Vector2(0, -3), defaultTraits);
        // SpawnAlly(new Vector2(2, 2), defaultTraits);
        // SpawnAlly(new Vector2(4, 3), defaultTraits);
        
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

    public IEnumerator StartCombat(int direction, int enemyCount, List<Trait> traits) {
        //show healthbar
        foreach(Drone g in livingAllies) g.GetComponent<AllyDrone>().showHealthbar();

        Vector3 spawnPoint = new Vector3();
        switch (direction) {
            case 0: 
                spawnPoint = transform.position + new Vector3(-12.5f, 0);
                break;
            case 1: 
                spawnPoint = transform.position + new Vector3(0, 8.5f);
                break;
            case 2: 
                spawnPoint = transform.position + new Vector3(12.5f, 0);
                break;
        }
        for (int i=0; i < enemyCount; i++) {
            StartCoroutine(SpawnEnemy(transform.position, traits, spawnPoint));
            yield return new WaitForSeconds(0.3f);
        }
        

        //TODO: come up with some logic on where we spawn enemies and what traits we will spawn them with
    }

    public void EndCombat() {
        //hide healthbar
        foreach(Drone g in livingAllies) g.GetComponent<AllyDrone>().hideHealthbar();

        //set to night
        GetComponent<DayNightCycleManager>().SetNight();
        OpenBreedingMenu();
    }

    IEnumerator SpawnEnemy(Vector2 pos, List<Trait> traits, Vector2 spawnPoint) {
        Drone enemy = SpawnDrone(enemyDroneObject, pos, traits);
        enemy.transform.position = spawnPoint;
        livingEnemies.Add(enemy);

        //update everyone's attack lists to fight each other
        foreach (Drone ally in livingAllies) enemy.attackList.Add(ally);

        yield return new WaitForSeconds(1);
        foreach (Drone ally in livingAllies) ally.attackList.Add(enemy);    //wait 1 second for enemy to aggro allies
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
    }

    public void OpenTraversalMenu() {
        if (traversalMenu == null) return;

        //make a grid of randomly generate tiles surrounding the current tile
        for (int i=0; i < 3; i++) {
            for (int j=0; j < 3; j++) {
                if (i == 1 && j == 1) continue;
                Instantiate(worldTiles[Random.Range(0, worldTiles.Length)]).transform.position = transform.position + new Vector3(-24 + 24 * i, -16 + 16 * j);
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
            case 0:
            //travel left
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(-24, 0, -10));
            break;

            case 1:
            //travel straight
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(0, 16, -10));
            break;

            case 2:
            //travel right
            GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(24, 0, -10));
            break;
        }
        CloseTraversalMenu();
        StartCoroutine(MoveDronesToCombat(choice, transform.position));
    }

    public IEnumerator MoveDronesToCombat(int direction, Vector3 startPosition) {

        for (int i=0; i < livingAllies.Count; i++) {

            if (direction == 0) {
                livingAllies[i].MoveToQueue(startPosition + new Vector3(-5.5f, 0));
                livingAllies[i].MoveToQueue(startPosition + new Vector3(-17, 0));

                int rowLimit = 7;
                int row = (int) (livingAllies.Count - i - 1) / rowLimit;
                Vector3 droneSlot = new Vector2(-19.5f - (1.5f * row), (1.5f * (i % rowLimit)) - ((Mathf.Min(rowLimit, livingAllies.Count) / 2) * 1.5f));
                livingAllies[i].MoveToQueue(startPosition + droneSlot);

                yield return new WaitForSeconds(0.2f);
            }

            if (direction == 1) {
                livingAllies[i].MoveToQueue(startPosition + new Vector3(0, 4));
                livingAllies[i].MoveToQueue(startPosition + new Vector3(0, 10));

                int rowLimit = 9;
                int row = (int) (livingAllies.Count - i - 1) / rowLimit;
                Vector3 droneSlot = new Vector2((1.5f * (i % rowLimit)) - ((Mathf.Min(rowLimit, livingAllies.Count) / 2) * 1.5f), 12 + (1.5f * row));
                livingAllies[i].MoveToQueue(startPosition + droneSlot);
                
                yield return new WaitForSeconds(0.2f);
            }

            if (direction == 2) {
                livingAllies[i].MoveToQueue(startPosition + new Vector3(5.5f, 0));
                livingAllies[i].MoveToQueue(startPosition + new Vector3(17, 0));

                int rowLimit = 7;
                int row = (int) (livingAllies.Count - i - 1) / rowLimit;
                Vector3 droneSlot = new Vector2(19.5f + (1.5f * row), (1.5f * (i % rowLimit)) - ((Mathf.Min(rowLimit, livingAllies.Count) / 2) * 1.5f));
                livingAllies[i].MoveToQueue(startPosition + droneSlot);

                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(6);
        StartCoroutine(StartCombat(direction, 5, new List<Trait>())); //TODO: change default values
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
