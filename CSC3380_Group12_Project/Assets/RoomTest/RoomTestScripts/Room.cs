using System.Collections;
using UnityEngine;

public class Room
{
    public GameObject room { private set; get; }
    public GameObject roomTeleporter { private set; get; }
    public GameObject mainRoomTeleporter { private set; get; }
    public GameObject roomCam { private set; get; }
    public UpgradeData upgradeReward { private set; get; }
    public GameObject[] enemies { private set; get; }
    public Room(GameObject room, GameObject roomTeleporter, GameObject mainRoomTeleporter, GameObject roomCam, UpgradeData upgradeReward, GameObject[] enemies)
    {
        this.room = room;
        this.roomTeleporter = roomTeleporter;
        this.mainRoomTeleporter = mainRoomTeleporter;
        this.roomCam = roomCam;
        this.upgradeReward = upgradeReward;
        this.enemies = enemies;

        mainRoomTeleporter.GetComponent<portalScript>().ActivatePortal();
    }

    public void delete()
    {
        Object.Destroy(room);
        mainRoomTeleporter.GetComponent<portalScript>().DeactivatePortal();
    }

    public bool portalIsLinked(GameObject portal)
    {
        return portal == mainRoomTeleporter;
    }

    
}