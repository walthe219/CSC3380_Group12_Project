using System.Collections;
using UnityEngine;

public class Room
{
    GameObject room;
    GameObject roomTeleporter;
    GameObject mainRoomTeleporter;
    GameObject roomCam;
    UpgradeData upgradeReward;

    public Room(GameObject room, GameObject roomTeleporter, GameObject mainRoomTeleporter, GameObject roomCam, UpgradeData upgradeReward)
    {
        this.room = room;
        this.roomTeleporter = roomTeleporter;
        this.mainRoomTeleporter = mainRoomTeleporter;
        this.roomCam = roomCam;
        this.upgradeReward = upgradeReward;
    }

    public void delete()
    {
        Object.Destroy(room);
    }
}