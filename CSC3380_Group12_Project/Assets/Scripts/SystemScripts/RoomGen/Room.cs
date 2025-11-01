using System.Collections;
using UnityEngine;

public class Room
{
    public GameObject room { private set; get; }
    public GameObject roomPortal { private set; get; }
    public GameObject mainRoomPortal { private set; get; }
    public GameObject roomCam { private set; get; }
    public UpgradeData upgradeReward { private set; get; }
    public GameObject[] enemies { private set; get; }

    public int roomNum;

    public static int roomsCreated = 0;

    public Room(GameObject room, GameObject roomPortal, GameObject mainRoomPortal, GameObject roomCam, UpgradeData upgradeReward, GameObject[] enemies)
    {
        this.room = room;
        this.roomPortal = roomPortal;
        this.mainRoomPortal = mainRoomPortal;
        this.roomCam = roomCam;
        this.upgradeReward = upgradeReward;
        this.enemies = enemies;
        
        room.gameObject.name = "Room " + roomNum;
        roomNum = roomsCreated;
        roomsCreated += 1;

        mainRoomPortal.GetComponent<portalScript>().ActivatePortal();
    }

    public void delete()
    {
        Object.Destroy(room);
        mainRoomPortal.GetComponent<portalScript>().DeactivatePortal();
    }

    public bool portalIsLinked(GameObject portal)
    {
        return portal == mainRoomPortal;
    }

    public override string ToString()
    {
        return room.gameObject.name;
    }



    
}