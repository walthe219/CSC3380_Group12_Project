using UnityEngine;

/*
 * Manages the creation of rooms, assignment of rooms to portals, assigment of upgrades to rooms, and deletion of rooms
 */
public class RoomManager : MonoBehaviour
{
    public string prefabFolderPath = "Tiles";
    public float roomGenDistance = 500;
    public float roomGenHeight = 0;
    public float maxTileRadius = 100;

    public GameObject teleporterPrefab;
    public GameObject cameraPrefab;


    public Transform mainRoomPostion;
    public GameObject[] mainRoomTeleporters;
    public GameObject[] mainRoomScreens;
    public GameObject[] mainRoomTextDisplays;
    private Room[] rooms = new Room[4];
    private Object[] prefab_arr;

    [SerializeField] Room currentlySelectedRoom;

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
            DontDestroyOnLoad(gameObject); // Persist across scene loads
        }
    }

    public void Start()
    {
        prefab_arr = Resources.LoadAll(prefabFolderPath, typeof(GameObject));
        RoomGenerator.initializePrefabs(cameraPrefab, teleporterPrefab);
        generateRoomTest();
    }
    public void generateNewRooms(int numRooms,float tileRadius, float gapSize, float roomHeight)
    {
        if (numRooms > 4) 
        {
            Debug.LogError("Can't generate more than 4 rooms");
        }


        UpgradeData[] potentialRoomRewards = UpgradeManager.Instance.samplePossibleUpgrades(4);
        Debug.Log("Room Rewards: " + ArrayHelper.print(potentialRoomRewards));

        for (int i = 0; i < numRooms; i++)
        {
            deleteRoom(rooms[i]);//deletes any rooms leftover

            // i=0 => (0,+z), i=1 => (+x,0) i=2 => (0,-z) i=3 => (-x,0)
            float xDir = (float)Mathf.Sin(Mathf.PI / 2 * i);
            float zDir = (float)Mathf.Cos(Mathf.PI / 2 * i);

            Vector3 roomPos = new Vector3(xDir*roomGenDistance,roomGenHeight,zDir*roomGenDistance);


            rooms[i] = RoomGenerator.CreateRoom(roomPos, prefab_arr, tileRadius, gapSize, roomHeight, mainRoomTeleporters[i], potentialRoomRewards[i]); //create new 
            mainRoomTextDisplays[i].GetComponent<TextDisplay>().changeText(potentialRoomRewards[i].ID);
        }
    }
    
    void deleteRoom(Room room)
    {
        if(room!=null) room.delete();
    }

    void deleteExcept(Room room)
    {
        foreach (Room other in rooms)
        {
            if(other!=room) other.delete();
        }
    }

    public void selectRoom()
    {
        currentlySelectedRoom = null;
        deleteExcept(currentlySelectedRoom);
    }
    
    [ContextMenu("generateRoomTest()")]
    public void generateRoomTest()
    {
        generateNewRooms(numRooms:4, tileRadius:10, gapSize:0, roomHeight:40);
    }
}
