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
    public Drone breedingTarget1;
    public Drone breedingTarget2;

    // Start is called before the first frame update
    void Start()
    {
        //spawn in 5 starter drones with default traits
        List<Trait> defaultTraits = new List<Trait>();
        
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

    void SpawnEnemy(Vector2 pos, List<Trait> traits) {
        GameObject enemy = SpawnDrone(enemyDroneObject, pos, traits);
        livingEnemies.Add(enemy);
    }

    void SpawnAlly(Vector2 pos, List<Trait> traits) {
        GameObject ally = SpawnDrone(allyDroneObject, pos, traits);
        livingAllies.Add(ally);
    }

    GameObject SpawnDrone(GameObject g, Vector2 pos, List<Trait> traits) {
        
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

    public void Breed() {
        breedingMenu.SetActive(false);

        //move them away from the group first? to a dedicated breeding area
        //smooching time
        breedingTarget1.MoveTo(breedingTarget2.transform.position);
        breedingTarget2.MoveTo(breedingTarget1.transform.position);
        //TODO: add heart animation
        //TODO: add wait time for full breeding animation to complete

        //average of the parents positions (for birth positions)
        Vector2 averagePos = (breedingTarget1.transform.position + breedingTarget2.transform.position) / 2f;

        List<Trait> traitList1 = breedingTarget1.GetTraits();
        List<Trait> traitList2 = breedingTarget2.GetTraits();
        List<Trait> sharedList = new List<Trait>();

        //find traits in common and remove them into their own list
        foreach(Trait t in traitList1) {
            if (traitList2.Contains(t)) {
                sharedList.Add(t);
                traitList1.Remove(t);
                traitList2.Remove(t);
            }
        }

        //create 3 baby drones
        for (int i = 0; i < 3; i++) {
            AllyDrone d = Instantiate(allyDroneObject).GetComponent<AllyDrone>();

            //100% chance for common traits
            d.AddTraits(sharedList); 

            //50% chance for all uncommon traits
            foreach (Trait t in traitList1) {
                if(Random.value > 0.5f) d.AddTrait(t);
            }
            foreach (Trait t in traitList2) {
                if(Random.value > 0.5f) d.AddTrait(t);
            }

            //set their birthing position and their move target
            d.transform.position = new Vector2(averagePos.x - 2 + (i * 2), averagePos.y - 1);
            d.MoveTo(new Vector2(0,0)); //TODO: move them back to the group after a small wait time
        }

        //reset breeding targets
        breedingTarget1 = null;
        breedingTarget2 = null;

        OpenBreedingMenu();
    }

    public void OpenTraversalMenu() {
        //TODO: traversal menu for choosing directions of travel with descriptions both specific and vague on what will be found there

        leftTile = worldTiles[Random.Range(0, worldTiles.Length)];
        middleTile = worldTiles[Random.Range(0, worldTiles.Length)];
        rightTile = worldTiles[Random.Range(0, worldTiles.Length)];

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
