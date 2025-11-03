using System.Collections;
using UnityEngine;


//Class containing all the data for a Room

public class Room
{
    public GameObject room { private set; get; } //Actual Room gameobject, contains all tiles, enemies, portal, camera
    public GameObject roomPortal { private set; get; } //Reference to room portal object in room
    public GameObject mainRoomPortal { private set; get; } //Reference to mainRoomPortal that is linked with this room's portal
    public GameObject roomCam { private set; get; } //Camera above room, can be used to display room preview
    public UpgradeData upgradeReward { private set; get; } //Reward Assigned to this room
    public GameObject[] enemies { private set; get; } //List of enemies spawned in this room


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

        //Activate portal in main, line can probably be moved to another class
        mainRoomPortal.GetComponent<portalScript>().ActivatePortal();
    }

    //All actions that are performed to delete a room 
    public void delete()
    {
        Object.Destroy(room);
        mainRoomPortal.GetComponent<portalScript>().DeactivatePortal();
    }

    //checks if the given portal is main room portal linked to this room
    public bool portalIsLinked(GameObject portal)
    {
        return portal == mainRoomPortal;
    }

    public override string ToString()
    {
        return room.gameObject.name;
    }



    
}