using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public static class RoomGenerator 
{
    /*
     * Creates a new Room using the paramters given in RoomManager
     */

    static GameObject camPrefab;
    static GameObject portalPrefab;
    static GameObject enemyPrefab;

    public static void initializePrefabs(GameObject cam, GameObject portal,GameObject enemy)
    {
        camPrefab = cam;
        portalPrefab = portal;
        enemyPrefab = enemy;
    }

    //Returns an object of class Room, creates room GameObject made of four tiles with a portal linked to main, and a list of enemies
    public static Room CreateRoom(Vector3 roomCenterPos, Object[] possibleTiles,float tileRadius,float gapSize,float roomHeight, GameObject portalLink, UpgradeData upgrade, List<GameObject> enemies)
    {

        GameObject room = new GameObject("Room");
        Object[] prefab_arr = ArrayHelper.Clone(possibleTiles);
        Stack<Object> tileStack = new Stack<Object>();
        GameObject[] placedTiles = new GameObject[4];
        NavMeshSurface surface = room.AddComponent<NavMeshSurface>();
        BoxCollider teleportCollider = room.AddComponent<BoxCollider>();
        TagSearcher search = new TagSearcher();


        //Place tiles down to create the room
        float totalRadius = gapSize / 2 + tileRadius;
        Vector3 pos = new Vector3(totalRadius, 0, -totalRadius);
    
        //Starting at the bottome left, add and rotate each tile
        // even => flip z  odd => flip x:
        // i = 0 => (-x, -z), i = 1 => (-x, z), i = 2 => (x, z), i = 3 => (x, -z)
        for (int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
            {
                pos.x *= -1;
            }
            else pos.z *= -1;

            placedTiles[i] = PlaceTile(getRandomTile(ref tileStack, prefab_arr), pos, 90 * i, room);
        }

        //Create Camera for the room preview
        GameObject roomCam = Object.Instantiate(camPrefab, Vector3.up * roomHeight, Quaternion.Euler(90, 0, 0), room.transform);
        //createCamera();


        //Create room portals and link to portals in main room
        Transform portalTransform = placedTiles[0].transform.Find("PortalPoint");
        if (portalTransform == null) {
            Debug.LogError($"Tile {placedTiles[0].name} does not have child named PortalPoint.");
        }
        GameObject roomPortal = Object.Instantiate(portalPrefab, portalTransform.position, portalTransform.localRotation, room.transform);
        roomPortal.tag = "isPortal";

        var roomPortalScript = roomPortal.GetComponent<portalScript>();
        roomPortalScript.LinkPortal(portalLink);
        room.transform.position = roomCenterPos;

        teleportCollider.center = new Vector3(0, -30f, 0);
        teleportCollider.size = new Vector3(100f, 0, 100f);

        /*GameObject[] linkStartPoints = search.search("LinkStartPoint", room.transform, 1);
        GameObject[] linkEndPoints = search.search("LinkEndPoint", room.transform, 1);*/

        GameObject[] linkStartPoints = GameObject.FindGameObjectsWithTag("LinkStartPoint");
        GameObject[] linkEndPoints = GameObject.FindGameObjectsWithTag("LinkEndPoint");

        NavMeshLink link;
        float nearest;
        float dist;
        GameObject nearestPoint;
        foreach (GameObject sPoint in linkStartPoints)
        {
            nearest = 10000;
            nearestPoint = null;
            foreach (GameObject ePoint in linkEndPoints)
            {
                dist = Vector3.Distance(sPoint.transform.position, ePoint.transform.position);

                if (dist < nearest)
                {
                    nearest = dist;
                    nearestPoint = ePoint;
                }
            }
            link = room.AddComponent<NavMeshLink>();
            if (link != null && nearestPoint != null)
            {
                link.startTransform = sPoint.transform;
                link.endTransform = nearestPoint.transform;
            }
            
        }

        //ADD NAVMESH HERE
        surface.BuildNavMesh();
        room.layer = LayerMask.NameToLayer("Ground");

        //Spawn enemies, set NavMesh target destination
        enemies = new List<GameObject>();
        foreach(GameObject tile in placedTiles)
        {
            foreach(Transform child in tile.transform)
            {
                if (child.CompareTag("EnemyPoint"))
                {
                    if (enemyPrefab.tag == "Runner")
                    {
                        enemyPrefab.GetComponent<RunnerBehavior>().portalTarget = portalTransform;
                        enemyPrefab.GetComponent<RunnerBehavior>().playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                    enemies.Add(Object.Instantiate(enemyPrefab,child.position,child.rotation, room.transform));
                    child.GameObject().SetActive(false);
                }
                if (child.CompareTag("PortalPoint") || child.CompareTag("ConnectionPoint"))
                {
                    child.GameObject().SetActive(false);
                }
            }
        }

        return new Room(room, roomPortal, portalLink, roomCam,upgrade,enemies.ToArray());

    }

    /*
     * Returns tile prebab from tileStack, and if empty randomly shuffles new tiles into the stack
     */
    private static GameObject getRandomTile(ref Stack<Object> tileStack, Object[]prefab_arr)
    {
        
        if (tileStack.Count == 0)
        {
            tileStack = new Stack<Object>(ArrayHelper.Shuffle(ArrayHelper.Clone(prefab_arr)));
        }
        GameObject tile = (GameObject)tileStack.Pop();
        return tile;
    }

    /*
     * Creates a new tile child of the room at some point and rotation
     */
    private static GameObject PlaceTile(GameObject tile, Vector3 offset, float rotation, GameObject room)
    {
        return Object.Instantiate(tile, offset, Quaternion.Euler(new Vector3(0, rotation, 0)), room.transform);
    }

    /*private static GameObject createCamera()
    {
        GameObject cameraObj = new GameObject("Room Camera");
        Camera cam = cameraObj.AddComponent<Camera>();
        RenderTexture texture = new RenderTexture(2000, 2000, 0);
        cam.targetTexture = texture;

        return cameraObj;
    }*/

    
}
