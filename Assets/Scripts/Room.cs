using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //references for the door objects
    [Header("Door Objects")]
    public Transform northDoor;
    public Transform southDoor;
    public Transform eastDoor;
    public Transform westDoor;

    //references for the wall objects
    [Header("Wall Objects")]
    public Transform northWall;
    public Transform southWall;
    public Transform eastWall;
    public Transform westWall;

    // how many tiles are there in the room?
    [Header("Values")]
    public int insideWidth;
    public int insideHeight;

    // objects to instantiate
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject coinPrefab;
    public GameObject healthPrefab;
    public GameObject keyPrefab;
    public GameObject exitDoorPrefab;

    // list of positions to avoid instantiating new objects at
    private List<Vector3> usedPositions = new List<Vector3>();

    public void GenerateInterior()
    {
        // create coins, enemies, health etc
        // do we spwan enemies?
        if(Random.value < Generation.instance.enemySpawnChance)
        {
            SpawnPrefab(enemyPrefab, 1, Generation.instance.maxEnemiesPerRoom + 1);
        }

        // do we spawn coins?
        if(Random.value < Generation.instance.coinSpawnChance)
        {
            SpawnPrefab(coinPrefab, 1, Generation.instance.maxCoinsPerRoom + 1);
        }

        // do we spawn health?
        if(Random.value < Generation.instance.healthSpawnChance)
        {
            SpawnPrefab(healthPrefab, 1, Generation.instance.maxHealthPerRoom + 1);
        }

    }

    public void SpawnPrefab(GameObject prefab, int min = 0, int max =0)
    {
        int num = 1;
        if(min != 0 || max != 0)
        {
            num = Random.Range(min, max);
        }

        // for each of the prefabs
        for(int x = 0; x < num; x++)
        {
            // intanciate the prefab
            GameObject obj = Instantiate(prefab);
            // getting the nearest tile to a random position inside the room
            Vector3 pos = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1), Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);

            // if the position is already in use, pick another random position.
            while(usedPositions.Contains(pos))
            {
                pos = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1), Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);
            }

            // place the prefab at the random position
            obj.transform.position = pos;
            // add the current position to the list of used positions
            usedPositions.Add(pos);

            // if the prefab we generated is enemyPrefab,
            if(prefab == enemyPrefab)
            {
                // add it to the EnemyManager's enemies list
                EnemyManager.instance.enemies.Add(obj.GetComponent<Enemy>());
                
            }
        }
    }
}
