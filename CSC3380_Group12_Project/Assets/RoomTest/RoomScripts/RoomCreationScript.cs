using UnityEngine;

public class RoomCreationScript : MonoBehaviour
{
    Object[] prefab_arr;

    public GameObject portalLink;
    public float cameraHeight;
    public GameObject camPrefab;
    public GameObject screen;
    public float componentRadius = 10;
    public Vector3 roomCenterPos = Vector3.zero;

    private GameObject room;
    private Camera roomCam;

    private void Start()
    {
        prefab_arr = Resources.LoadAll("Subfloors", typeof(GameObject));

        room = new GameObject("Test Generated Room");

        Vector3 pos = new Vector3(componentRadius, 0, -componentRadius);
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

            PlaceSubroom((GameObject)(prefab_arr[i+1]), pos, 90 * i, room);
        }

        Instantiate(camPrefab, Vector3.up * cameraHeight, Quaternion.identity, room.transform);
    }

    private void Update()
    {
        if (room.transform.position.Equals(Vector3.zero))
        {
            room.transform.position = roomCenterPos;
            Debug.Log("Room moved to " + room.transform.position);
        }
    }

    private void PlaceSubroom(GameObject subfloor,Vector3 offset, float rotation, GameObject room)
    {
        Instantiate(subfloor, offset, Quaternion.Euler(new Vector3(0,rotation,0)),room.transform);
    }

}
