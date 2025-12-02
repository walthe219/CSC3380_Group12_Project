using System.Collections.Generic;
using Unity.AI.Navigation;
using System;
using UnityEngine;

public static class RoomGenerator
{
    /*
     * Creates a new Room using the paramters given in RoomManager
     */

    static GameObject camPrefab;
    static GameObject portalPrefab;
    static GameObject enemyPrefab;
    static GameObject fallPlanePrefab;

    public static event Action<GameObject> TouchedFallPlane;

    public static void initializePrefabs(GameObject cam, GameObject portal, GameObject enemy, GameObject fallplane)
    {
        camPrefab = cam;
        portalPrefab = portal;
        enemyPrefab = enemy;
        fallPlanePrefab = fallplane;
    }

    //Returns an object of class Room, creates room GameObject made of four tiles with a portal linked to main, and a list of enemies
    public static Room CreateRoom(Vector3 roomCenterPos, float tileRadius, float gapSize, float roomHeight, GameObject portalLink, UpgradeData upgrade, List<GameObject> enemies)
    {

        GameObject room = new GameObject("Room");
        GameObject[] placedTiles = new GameObject[4];
        TagSearcher search = new TagSearcher();

        //Place tiles down to create the room
        (GameObject, Stack<GameObject>)[] selectedTiles = TilePooling.pullRandonTiles(4);

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

            GameObject tile = selectedTiles[i].Item1;

            placedTiles[i] = PlaceTile(tile, pos, 90 * i, room);
        }

        //Create Camera for the room preview
        GameObject roomCam = GameObject.Instantiate(camPrefab, Vector3.up * roomHeight, Quaternion.Euler(90, 0, 0), room.transform);
        //createCamera();


        //Create room portals and link to portals in main room
        Transform portalTransform = placedTiles[0].transform.Find("PortalPoint");
        if (portalTransform == null)
        {
            Debug.LogError($"Tile {placedTiles[0].name} does not have child named PortalPoint.");
        }
        GameObject roomPortal = GameObject.Instantiate(portalPrefab, portalTransform.position, portalTransform.localRotation, room.transform);
        roomPortal.tag = "isPortal";

        var roomPortalScript = roomPortal.GetComponent<portalScript>();
        roomPortalScript.LinkPortal(portalLink);


        //create fall plane
        GameObject fallPlane = GameObject.Instantiate(fallPlanePrefab, Vector3.down * roomHeight, Quaternion.identity, room.transform);
        float planeSize = tileRadius * 20 + gapSize;
        fallPlane.GetComponent<BoxCollider>().size = new Vector3(planeSize, 0, planeSize);

        //link fall plane to room portal
        portalScript fallPortalScript = fallPlane.GetComponent<portalScript>();
        fallPortalScript.setDestination(roomPortal.transform);
        fallPortalScript.PlayerEnterPortal += (PARAMETER_NOT_NEEDED) => { Debug.Log("Player fell off tiles");};
        fallPortalScript.PlayerEnterPortal += TouchedFallPlane;


        //Move room to correct position
        room.transform.position = roomCenterPos;

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
        NavMeshSurface surface = room.AddComponent<NavMeshSurface>();
        room.layer = LayerMask.NameToLayer("Ground");
        return new Room(room, selectedTiles, roomPortal, portalLink, null, upgrade, enemies.ToArray(), surface);

    }


    /*
     * Creates a new tile child of the room at some point and rotation
     */
    private static GameObject PlaceTile(GameObject tile, Vector3 offset, float rotation, GameObject room)
    {
        tile.transform.parent = room.transform;
        tile.transform.position = offset;
        tile.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        tile.SetActive(true);
        return tile;
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