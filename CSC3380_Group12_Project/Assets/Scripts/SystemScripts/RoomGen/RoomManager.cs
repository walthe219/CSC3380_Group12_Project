using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Manages the creation of rooms, assignment of rooms to portals, assigment of upgrades to rooms, and deletion of rooms
 */
public class RoomManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    [SerializeField] string prefabFolderPath = "Tiles";
    [SerializeField] Vector3 poolingLocation = Vector3.zero;
    //[SerializeField] UnityEngine.Object[] prefab_arr;

    [Header("RoomGen")]
    [SerializeField] float roomGenDistance = 500;
    [SerializeField] float roomGenHeight = 0;
    [SerializeField] float maxTileRadius = 100;
    [SerializeField] int numRoomsToCreate = 4;

    [Header("Main Room")]
    [SerializeField] Transform mainRoomPostion;
    [SerializeField] GameObject[] mainRoomPortals;
    [SerializeField] GameObject[] mainRoomScreens;
    [SerializeField] GameObject[] mainRoomTextDisplays;

    [Header("Prefabs")]
    [SerializeField] GameObject portalPrefab;
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject fallPlanePrefab;


    //maybe in refactor put these into new class
    //-------------------------------------------
    [Header("Debug")]
    [SerializeField] Room[] rooms;
    [SerializeField] Room currentlySelectedRoom;
    [SerializeField] int currentEnemiesAlive;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] List<GameObject> enemies;
    //[SerializeField] string[] roomNames;
    //------------------------------------------



    //Events, names are not intutive should change these maybe
    public event Action<string> PassUpgradeId;
    public event Action<int> PassEnemiesAlive;
    public event Action RoomCleared;
    public event Action<string> RecieveReward;

    //makes RoomManager a singelton class, a static MonoBehaviour
    public static RoomManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        GameObject manage = GameObject.FindGameObjectWithTag("ManagerParent");
        NavMeshSurface initSurf = manage.AddComponent<NavMeshSurface>();
        initSurf.BuildNavMesh();
    }

    private void Start()
    {
        TilePooling.initialize(prefabFolderPath, poolingLocation);
        RoomGenerator.initializePrefabs(cameraPrefab, portalPrefab, enemyPrefab, fallPlanePrefab);
        Array.ForEach(mainRoomPortals, obj => obj.GetComponent<portalScript>().PlayerEnterPortal += selectLinkedRoom);
        generateNewRooms(numRooms: numRoomsToCreate, tileRadius: maxTileRadius, gapSize: 0, roomHeight: 40);
    }
    void generateNewRooms(int numRooms, float tileRadius, float gapSize, float roomHeight)
    {
        if (numRooms > 4)
        {
            Debug.LogError("Can't generate more than 4 rooms");
        }

        //deletes any rooms leftover if they exist
        if (rooms != null)
        {
            Debug.Log("Destroying leftover rooms");
            Array.ForEach(rooms, room => { if (room != null) deleteRoom(room); });
        }
        rooms = new Room[numRooms];

        //Direct Reference to Singleton UpgradeManager, UpgradeManager needed in scene for RoomManager to run, should try to use observer design pattern instead
        UpgradeData[] potentialRoomRewards = UpgradeManager.Instance.samplePossibleUpgrades(4);
        Debug.Log("Room Rewards: " + ArrayHelper.print(potentialRoomRewards));

        for (int i = 0; i < numRooms; i++)
        {

            // i=0 => (0,+z), i=1 => (+x,0) i=2 => (0,-z) i=3 => (-x,0)
            float xDir = (float)Mathf.Sin(Mathf.PI / 2 * i);
            float zDir = (float)Mathf.Cos(Mathf.PI / 2 * i);

            Vector3 roomPos = new Vector3(xDir * roomGenDistance, roomGenHeight, zDir * roomGenDistance);

            TextDisplay display = mainRoomTextDisplays[i].GetComponent<TextDisplay>();
            UpgradeData roomReward = potentialRoomRewards[i];
            display.changeText(roomReward.ID, roomReward.printDescription(ID:false,label:false));
            rooms[i] = RoomGenerator.CreateRoom(roomPos, tileRadius, gapSize, roomHeight, mainRoomPortals[i], potentialRoomRewards[i], enemies); //create new 
        }
    }

    void deleteRoom(Room room)
    {
        if (room != null) room.delete();
    }

    void deleteExcept(Room room)
    {
        foreach (Room other in rooms)
        {
            if (other != room) other.delete();
        }
        rooms = new Room[1];
        rooms[0] = room;
    }

    void selectLinkedRoom(GameObject mainRoomPortal)
    {
        Debug.Log("Selecting Room");
        foreach (Room room in rooms)
        {
            if (room.portalIsLinked(mainRoomPortal))
            {

                currentlySelectedRoom = room;
                break;
            }
        }

        deleteExcept(currentlySelectedRoom);
        currentlySelectedRoom.surface.BuildNavMesh();

        enemies = new List<GameObject>();
        foreach (Transform tile in currentlySelectedRoom.room.transform)
        {
            foreach (Transform child in tile.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("EnemyPoint"))
                {
                    //GameObject spawned = ObjectPooler.Reference.SpawnFromPool(enemyPrefab.tag, child.position, child.rotation, currentlySelectedRoom.room);
                    GameObject spawned = GameObject.Instantiate(enemyPrefab, child.position, child.rotation, currentlySelectedRoom.room.transform);

                    if (spawned.tag == "Runner")
                    {
                        spawned.GetComponent<RunnerBehavior>().portalTarget = currentlySelectedRoom.roomPortal.transform;
                        spawned.GetComponent<RunnerBehavior>().playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                    enemies.Add(spawned);
                    child.gameObject.SetActive(false);
                }
                if (child.CompareTag("PortalPoint") || child.CompareTag("ConnectionPoint") || child.CompareTag("LinkStartPoint") || child.CompareTag("LinkEndPoint"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        currentlySelectedRoom.reassignEnemies(enemies);

        currentEnemiesAlive = currentlySelectedRoom.enemies.Length;
        PassUpgradeId?.Invoke(currentlySelectedRoom.upgradeReward.ID);
        PassEnemiesAlive?.Invoke(currentEnemiesAlive);
        Array.ForEach(currentlySelectedRoom.enemies, enemy => enemy.GetComponent<Target>().OnDeath += decrementEnemies);
        currentlySelectedRoom.roomPortal.GetComponent<portalScript>().DeactivatePortal();
    }

    void decrementEnemies()
    {
        currentEnemiesAlive--;
        Debug.Log("numEnemies: " + currentEnemiesAlive);
        PassEnemiesAlive?.Invoke(currentEnemiesAlive);
        if (currentEnemiesAlive == 0)
        {
            OnRoomClear();
        }
    }

    void OnRoomClear()
    {
        Debug.Log("Room Cleared! Go back to the portal and return to the main room for your reward");
        RoomCleared?.Invoke();
        currentlySelectedRoom.roomPortal.GetComponent<portalScript>().ActivatePortal();

        RecieveReward?.Invoke(currentlySelectedRoom.upgradeReward.ID);
        PassUpgradeId?.Invoke("Recieved");

        currentlySelectedRoom.roomPortal.GetComponent<portalScript>().PlayerEnterPortal += ResetFields;
        currPlayerStats.numRoomsComp++;
    }

    //called when enter main room
    void ResetFields(GameObject NOTUSED)
    {
        PassUpgradeId?.Invoke("None");
        deleteRoom(currentlySelectedRoom);
        rooms = null;
        currentlySelectedRoom = null;
        generateRoomTest();

    }

    [ContextMenu("generateRoomTest()")]
    public void generateRoomTest()
    {
        generateNewRooms(numRooms: numRoomsToCreate, tileRadius: maxTileRadius, gapSize: 0, roomHeight: 40);
    }
}