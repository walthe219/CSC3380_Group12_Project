using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Room
{
    public GameObject room { private set; get; } //Actual Room gameobject, contains all tiles, enemies, portal, camera
    public (GameObject, Stack<GameObject>)[] tiles { private set; get; }
    public GameObject roomPortal { private set; get; } //Reference to room portal object in room
    public GameObject mainRoomPortal { private set; get; } //Reference to mainRoomPortal that is linked with this room's portal
    public GameObject roomCam { private set; get; } //Camera above room, can be used to display room preview
    public UpgradeData upgradeReward { private set; get; } //Reward Assigned to this room
    public GameObject[] enemies { private set; get; } //List of enemies spawned in this room
    public NavMeshSurface surface { private set; get; }


    public int roomNum;

    public static int roomsCreated = 0;

    public Room(GameObject room, (GameObject, Stack<GameObject>)[] tiles, GameObject roomPortal, GameObject mainRoomPortal, GameObject roomCam, UpgradeData upgradeReward, GameObject[] enemies, NavMeshSurface surface)
    {
        this.room = room;
        this.tiles = tiles;
        this.roomPortal = roomPortal;
        this.mainRoomPortal = mainRoomPortal;
        this.roomCam = roomCam;
        this.upgradeReward = upgradeReward;
        this.enemies = enemies;
        this.surface = surface;

        room.gameObject.name = "Room " + roomNum;
        roomNum = roomsCreated;
        roomsCreated += 1;

        mainRoomPortal.GetComponent<portalScript>().ActivatePortal();
    }

    public void delete()
    {
        foreach (var tuple in tiles)
        {
            var tile = tuple.Item1;
            var tileStack = tuple.Item2;
            TilePooling.reclaimTile(tile, tileStack);
        }
        Object.Destroy(room);
        mainRoomPortal.GetComponent<portalScript>().DeactivatePortal();
    }

    public bool portalIsLinked(GameObject portal)
    {
        return portal == mainRoomPortal;
    }

    public void reassignEnemies(List<GameObject> list)
    {
        enemies = list.ToArray();
    }

    public override string ToString()
    {
        return room.gameObject.name;
    }




}