using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [Header("Map Size")]
    public int mapWidth = 7;
    public int mapHeight = 7;
    public int roomsToGenerate = 12;

    private int roomCount;
    private bool roomsInstantiated;

    // store our first room's position for procedural level generation
    private Vector2 firstRoomPos;

    // A 2D boolean array to map out the level
    private bool[,] map;
    // the room prefab to instantiate 
    [Header("Room Prefab")]
    public GameObject roomPrefab;

    // List of class type Room
    private List<Room> roomObjects = new List<Room>();

    // singleton
    public static Generation instance;

    [Header("Spawn Chance")]
    public float enemySpawnChance;
    public float coinSpawnChance;
    public float healthSpawnChance;

    [Header("Spawn Amount Per Room")]
    public int maxEnemiesPerRoom;
    public int maxCoinsPerRoom;
    public int maxHealthPerRoom;

    void Awake ()
    {
        instance = this;
    }

    // void Start ()
    // {
    //     // random seed assigned to a random number generator
    //     // Random.InitState(74742584);
    //     Generate();
    // }

    public void OnPlayerMove ()
    {
        // get the position of the player
        Vector2 playerPos = FindObjectOfType<Player>().transform.position;
        // get the position of the room that the player is in, in terms of map scale.
        Vector2 roomPos = new Vector2(((int)playerPos.x + 6) / 12, ((int)playerPos.y + 6) / 12);

        // generate a newer version of the map
        UI.instance.map.texture = MapTextureGenerator.Generate(map, roomPos);
    }

    // called at the start of the game - begins the generation process
    public void Generate ()
    {
        // create a new map of the specified size
        map = new bool[mapWidth, mapHeight];
        // check to see if we can place a room in the center of the map. Since this is the first room, there will be no branch nor a general direction.
        CheckRoom(3, 3, 0, Vector2.zero, true);
        // instantiate the rooms
        InstantiateRooms();
        // Find the player in the scene, and position them inside the first room.
        FindObjectOfType<Player>().transform.position = firstRoomPos * 12;

        // apply the texture to the raw image minimap.
        UI.instance.map.texture = MapTextureGenerator.Generate(map, firstRoomPos);
    }

    // checks to see if we can place a room here - continues the branch in the generalDirection.
    void CheckRoom (int x, int y, int remaining, Vector2 generalDirection, bool firstRoom = false)
    {
        // if we have generated all of the rooms that we need, stop checking the rooms.
        if(roomCount >= roomsToGenerate)
        {
            return;
        }

        // if this is outside the bounds of the actual map, stop the function.
        if(x < 0 || x >= mapWidth -1 || y < 0 || y >= mapHeight -1)
        {
            return;
        }

        // if this is not the first room, and there is no more room to check, stop the function.
        if(firstRoom == false && remaining <= 0)
        {
            return;
        }
        
        // if the given map tile is already occupied, stop the function.
        if(map[x, y] == true)
        {
            return;
        }

        // if this is the first room, store the room position.
        if(firstRoom == true)
        {
            firstRoomPos = new Vector2(x, y);
        }

        // add one to roomCount and set the map tile to be true.
        roomCount++;
        map[x, y] = true;

        // 'north' will be true if the random value is greater than ...
        bool north = Random.value > (generalDirection == Vector2.up ? 0.2f : 0.8f);
        // 'south' will be true if the random value is greater than ...
        bool south = Random.value > (generalDirection == Vector2.down ? 0.2f : 0.8f);
        // 'east' will be true if the random value is greater than ...
        bool east = Random.value > (generalDirection == Vector2.right ? 0.2f : 0.8f);
        // 'west' will be true if the random value is greater than ...
        bool west = Random.value > (generalDirection == Vector2.left ? 0.2f : 0.8f);

        int maxRemaining = roomsToGenerate / 4;

        // if north is true, make a room one tile above the current.
        if(north == true || firstRoom)
        {
            CheckRoom(x, y + 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.up : generalDirection);
        }
        // if south is true, make a room one tile below the current.
        if(south == true || firstRoom)
        {
            CheckRoom(x, y - 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.down : generalDirection);
        }
        // if east is true, make a room one tile to the right of the current.
        if(east == true || firstRoom)
        {
            CheckRoom(x + 1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.right : generalDirection);
        }
        // if west is true, make a room one tile to the left of the current.
        if(west == true || firstRoom)
        {
            CheckRoom(x - 1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.left : generalDirection);
        }

    }

    // once the rooms have been decided, they will be spawned in and setup
    void InstantiateRooms ()
    {
        // call this function once
        if(roomsInstantiated == true)
        {
            return;
        }

        // set roomsInstantiated to true
        roomsInstantiated = true;

        // loop through each element inside of our map array
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                // if the map tile doesn't exist, skip over to the next one.
                if(map[x, y] == false)
                {
                    continue;
                }

                // instantiate a new room prefab
                GameObject roomObj = Instantiate(roomPrefab, new Vector3(x , y , 0) * 12, Quaternion.identity);
                // get a reference to the Room script of the new room object
                Room room = roomObj.GetComponent<Room>();

                // if we're within the boundary of the map, AND if there is room above us
                if(y < mapHeight - 1 && map[x, y + 1] == true)
                {
                    // enable the north door and disable the north wall.
                    room.northDoor.gameObject.SetActive(true);
                    room.northWall.gameObject.SetActive(false);
                }

                // if we're within the boundary of the map, AND if there is room below us
                if(y > 0 && map[x, y - 1] == true)
                {
                    // enable the south door and disable the south wall.
                    room.southDoor.gameObject.SetActive(true);
                    room.southWall.gameObject.SetActive(false);
                }

                // if we're within the boundary of the map, AND if there is room to the right of us
                if(x < mapWidth - 1 && map[x + 1, y] == true)
                {
                    // enable the east door and disable the east wall.
                    room.eastDoor.gameObject.SetActive(true);
                    room.eastWall.gameObject.SetActive(false);
                }

                // if we're within the boundary of the map, AND if there is room to the left of us
                if(x > 0 && map[x - 1, y] == true)
                {
                    // enable the west door and disable the west wall.
                    room.westDoor.gameObject.SetActive(true);
                    room.westWall.gameObject.SetActive(false);
                }

                // if this is not the first room, call GenerateInterior().
                if(firstRoomPos != new Vector2(x, y))
                {
                    room.GenerateInterior();
                }

                // add the room to the roomObjects list
                roomObjects.Add(room);
            }
        }

        // after looping through every element inside the map array, call CalculateKeyAndExit().
        CalculateKeyAndExit();
    }

    // places the key and exit in the level
    void CalculateKeyAndExit()
    {
        float maxDist = 0;
        Room a = null;
        Room b = null;

        foreach(Room aRoom in roomObjects)
        {
            foreach(Room bRoom in roomObjects)
            {
                // compare each of the rooms to find out which pair is the furtherest away.
                float dist = Vector3.Distance(aRoom.transform.position, bRoom.transform.position);
                if(dist > maxDist)
                {
                    a = aRoom;
                    b = bRoom;
                    maxDist = dist;
                }
            }

        }
            // once room A and room B are found, spawn in the key and the exitdoor.
            a.SpawnPrefab(a.keyPrefab);
            b.SpawnPrefab(b.exitDoorPrefab);
    }

}
