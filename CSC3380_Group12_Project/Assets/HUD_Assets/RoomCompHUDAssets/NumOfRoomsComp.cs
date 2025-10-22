using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumOfRoomsComp : MonoBehaviour
{

    //In this class, we will want to use an event system. First, whoever is making the script that keeps track of enemies, we create an event with them that has 
    //Action<int> so that when a room is cleared by having all enemies killed, they can invoke an event with the number of rooms completed and I can create a local
    //rooms completed variable that will be linked to the UI

    private int finishedRooms;
    private int abc;
    public TextMeshProUGUI finishedRoomsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishedRooms = 0;
        finishedRoomsText = GameObject.Find("NumOfRooms").GetComponent<TextMeshProUGUI>();
    }

    void NORToText(){
        if(finishedRoomsText != null){
            finishedRoomsText.text = finishedRooms.ToString();
        }
        else{
            Debug.Log("finishedRooms is null");
        }
    }

    public int setFinishedRooms(int abc){
        this.abc = finishedRooms;
        return finishedRooms;
    }

    public int finishedRoomsInc(){
        //if(all enemies dead) then ->
        finishedRooms++;
        return finishedRooms;
    }

    // Update is called once per frame
    void Update()
    {
        NORToText();
    }
}
