using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumOfRoomsComp : MonoBehaviour
{

    //In this class, we will want to use an event system. First, whoever is making the script that keeps track of enemies, we create an event with them that has 
    //Action<int> so that when a room is cleared by having all enemies killed, they can invoke an event with the number of rooms completed and I can create a local
    //rooms completed variable that will be linked to the UI

    public float numRoomsComp;
    private int finishedRooms;
    private float abc;
    public TextMeshProUGUI finishedRoomsText;
    [SerializeField] PlayerStats CurrentPlayerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not assigned in insepctor (NumOfRooomsComp)");
        }
        CurrentPlayerStats.numRoomsComp = 0;
        finishedRoomsText = GameObject.Find("NumOfRooms").GetComponent<TextMeshProUGUI>();
    }

    void NORToText(){
        if(finishedRoomsText != null){
            finishedRoomsText.text = CurrentPlayerStats.numRoomsComp.ToString();
        }
        else{
            Debug.Log("finishedRooms is null");
        }
    }

    public float setFinishedRooms(float abc){
        this.abc = CurrentPlayerStats.numRoomsComp;
        return CurrentPlayerStats.numRoomsComp;
    }

    public float finishedRoomsInc(){
        //if(all enemies dead) then ->
        CurrentPlayerStats.numRoomsComp++;
        return CurrentPlayerStats.numRoomsComp;
    }

    public float finishedRoomsDec(){ //test func
        CurrentPlayerStats.numRoomsComp--;
        return CurrentPlayerStats.numRoomsComp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            finishedRoomsInc();
        }
        if (Input.GetKeyDown(KeyCode.N)){
            finishedRoomsDec();
        }
        
        NORToText();
    }
}
