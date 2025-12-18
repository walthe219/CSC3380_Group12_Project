using System.Collections.Generic;
using UnityEngine;

/*
 * Creates a Room gameobject from tile prefabs stored in Assets/Resources/SomeFolderName
 * utilizes ArrayHelper class I createed
 */
public class RoomCreationScript : MonoBehaviour
{
    //Room Creation Prameters
    public string prefabFolderName = "Tiles";
    public Vector3 roomCenterPos = Vector3.zero; //where the room will be created in the world
    public float height; //determines height of camera, could be used for wall and ceiling height
    public float maxTileRadius = 10; //max radius of tile prefabs, should all be within max tile size
    public float gapSize = 0; //additional distance added between tiles

    public float pitSize = 1; //not implemented
    public bool useRoomPrefab = false; //not implemented
    public bool createRoomWalls = false;//not implemented
    public bool createRoomCeiling = false;//not implemented
    public bool createRoomPitFloor = false;//not implemented

    
    public GameObject portalLink; //not implemented, reference to portal in center room to connect to new room
    public GameObject camPrefab; //used to create camera for room preview
    public GameObject screen; //not implemented, screen to display cam on

    private GameObject room; 
    private Camera roomCam;
    private Object[] prefab_arr;
    private Stack<Object> tileStack = new Stack<Object>();

    private void Start()
    {
        // Note, this class creates a memory leak, probably from the Resources.LoadAll method if i were to guess
        prefab_arr = Resources.LoadAll(prefabFolderName, typeof(GameObject));
        CreateRoom();
    }

    /*
     * Returns tile prebab from tileStack, and if empty randomly shuffles new tiles into the stack
     */
    private GameObject getRandomTile()
    {
        if(tileStack.Count == 0)
        {
            tileStack = new Stack<Object>(ArrayHelper.Shuffle(ArrayHelper.Clone(prefab_arr)));
        }
        return (GameObject)tileStack.Pop();
    }

    /*
     * Creates a new tile child of the room at some point and rotation
     */
    private void PlaceTile(GameObject tile,Vector3 offset, float rotation, GameObject room)
    {
        Instantiate(tile, offset, Quaternion.Euler(new Vector3(0,rotation,0)),room.transform);
    }

    /*
     * Creates a new Room using the paramters set in the editor 
     */
    [ContextMenu("CreateRoom()")]
    private void CreateRoom()
    {
        room = new GameObject("Test Generated Room");
        float totalRadius = gapSize/2 + maxTileRadius;
        Vector3 pos = new Vector3(totalRadius, 0, -totalRadius);

        //Starting at the bottome left, add and rotate each tile
        for (int i = 0; i < 4; i++)
        {
            // i=0 (-x,-x), i=1 (-x,x), i=2 (x,x), i=3 (x,-x)
            //even = z
            //odd = x
            if (i % 2 == 0)
            {
                pos.x *= -1;
            }
            else pos.z *= -1;

            PlaceTile(getRandomTile(), pos, 90 * i, room);
        }
        Instantiate(camPrefab, Vector3.up * height, Quaternion.Euler(90, 0, 0), room.transform);

        room.transform.position = roomCenterPos;


    }
}
