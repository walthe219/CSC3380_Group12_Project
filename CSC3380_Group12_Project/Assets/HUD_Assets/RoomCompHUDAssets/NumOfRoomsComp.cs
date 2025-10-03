using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumOfRoomsComp : MonoBehaviour
{

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
        finishedRooms++;
        return finishedRooms;
    }

    // Update is called once per frame
    void Update()
    {
        NORToText();
    }
}
