using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager_Script : MonoBehaviour
{

    public static Game_Manager_Script instance;

    public GameObject playerGO;                             // Get the player GO from prefabs folder
    public GameObject healthPickUpGO;
    public GameObject ammoPickUpGO;
    public GameObject dropShipGO;

    public GameObject alienOgerGO;
    public GameObject alienInsectGO;
    
    public int playerCredits = 0;
    public int spawnNumEnemies = 1;
    public int currentNumEnemies;

    void Awake()                                            // First function to run in scene
    {
        instance = this;                                    // This object is the only instance of Game_Controller_Script
    }

    // Use this for initialization
    void Start()
    {
        SpawnDropShip();
       Instantiate(playerGO, new Vector3(-11, 1, 1), Quaternion.Euler(0, 0, 0));         // Create player in scene
    }

    public void SpawnPickUps()
    {
        GameObject[] ammoGO = GameObject.FindGameObjectsWithTag("MaxAmmo") as GameObject[];
        GameObject[] healthGO = GameObject.FindGameObjectsWithTag("MaxHealth") as GameObject[];

        for (var i = 0; i < ammoGO.Length; i++)
        {
            Destroy(ammoGO[i]);
        }
        for (var i = 0; i < healthGO.Length; i++)
        {
            Destroy(healthGO[i]);
        }

        Instantiate(healthPickUpGO, new Vector3(-12, 2.5f, 28), Quaternion.Euler(0, 0, 90));
        Instantiate(ammoPickUpGO, new Vector3(-12, 2.5f, -28), Quaternion.Euler(0, 0, 90));
    }
    public void SpawnDropShip()
    {
        SpawnPickUps();
        Instantiate(dropShipGO, new Vector3(-26, 500, 0), Quaternion.identity);
    }
    public void SpawnEnemy()
    {
        for (int i = 0; i < spawnNumEnemies; i++)
        {
            int rndNum = Random.Range(1,3);
            switch (rndNum)
            {
                case 1:
                    Instantiate(alienOgerGO, new Vector3(-26, 0, 0), Quaternion.Euler(0, 0, 0));
                    break;
                default:
                    Instantiate(alienInsectGO, new Vector3(-26, 0, 0), Quaternion.Euler(0, 0, 0));
                    break;
            }            
            currentNumEnemies++;
        }
        // Debug.Log("Number of enemies: " + currentNumEnemies);
    }
}