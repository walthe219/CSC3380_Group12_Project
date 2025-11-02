using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumOfRoomsComp : MonoBehaviour
{

    //In this class, we will want to use an event system. First, whoever is making the script that keeps track of enemies, we create an event with them that has 
    //Action<int> so that when a room is cleared by having all enemies killed, they can invoke an event with the number of rooms completed and I can create a local
    //rooms completed variable that will be linked to the UI

    private float RoomsComp;
    private int finishedRooms;
    private float abc;
    public TextMeshProUGUI finishedRoomsText;
    //private PlayerStats playerstats;
    [SerializeField] PlayerStats playerstats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerstats = new PlayerStats();
        playerstats.numRoomsComp = 0;
        finishedRoomsText = GameObject.Find("NumOfRooms").GetComponent<TextMeshProUGUI>();
    }

    void NORToText(){
        if(finishedRoomsText != null){
            finishedRoomsText.text = playerstats.numRoomsComp.ToString();
        }
        else{
            Debug.Log("finishedRooms is null");
        }
    }

    public float setFinishedRooms(float abc){
        this.abc = playerstats.numRoomsComp;
        return playerstats.numRoomsComp;
    }

    public float finishedRoomsInc(){
        //if(all enemies dead) then ->
        playerstats.numRoomsComp++;
       return playerstats.numRoomsComp;
    }

    public float finishedRoomsDec(){ //test func
        playerstats.numRoomsComp--;
        return playerstats.numRoomsComp;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M)){
            //finishedRoomsInc();
        //}
        //if (Input.GetKeyDown(KeyCode.N)){
            //finishedRoomsDec();
        //}
        //Debug.Log("Rooms Completed is: " + RoomsComp);
        
        NORToText();
    }
}
