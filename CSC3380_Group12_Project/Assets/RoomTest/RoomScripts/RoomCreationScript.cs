using System.Collections.Generic;
using UnityEngine;

public class RoomCreationScript : MonoBehaviour
{
    public GameObject portalLink;
    public GameObject camPrefab;
    public GameObject screen;

    public float height;
    public float componentRadius = 10;
    public float gapSize = 0;
    public float pitSize = 1;

    public bool useRoomPrefab = false;
    public bool createRoomWalls = false;
    public bool createRoomCeiling = false;
    public bool createRoomPitFloor = false;

    public Vector3 roomCenterPos = Vector3.zero;

    private GameObject room;
    private Camera roomCam;
    private Object[] prefab_arr;
    private Stack<Object> tileStack = new Stack<Object>();

    private void Start()
    {
        prefab_arr = Resources.LoadAll("Subfloors", typeof(GameObject));
        CreateRoom();
    }

    private Object getRandomTile()
    {
        if(tileStack.Count == 0)
        {
            tileStack = new Stack<Object>(ArrayHelper.Shuffle(ArrayHelper.Clone(prefab_arr)));
        }
        return tileStack.Pop();
    }

    private void PlaceSubroom(GameObject subfloor,Vector3 offset, float rotation, GameObject room)
    {
        Instantiate(subfloor, offset, Quaternion.Euler(new Vector3(0,rotation,0)),room.transform);
    }

    [ContextMenu("CreateRoom()")]
    private void CreateRoom()
    {
        room = new GameObject("Test Generated Room");
        float totalRadius = gapSize/2 + componentRadius;
        Vector3 pos = new Vector3(totalRadius, 0, -totalRadius);
        for (int i = 0; i < 4; i++)
        {
            // 0(-x,-x), 1(-x,x), 2(x,x), 3(x,-x)

            //even = z
            //odd = x
            if (i % 2 == 0)
            {
                pos.x *= -1;
            }
            else pos.z *= -1;

            PlaceSubroom((GameObject)(getRandomTile()), pos, 90 * i, room);
        }
        Instantiate(camPrefab, Vector3.up * height, Quaternion.Euler(90, 0, 0), room.transform);

        room.transform.position = roomCenterPos;


    }
}
