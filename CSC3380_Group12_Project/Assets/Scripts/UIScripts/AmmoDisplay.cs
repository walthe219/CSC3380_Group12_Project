using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoDisplay;
    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;

    void Start()
    {   
        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not assigned in insepctor (AmmoDisplay)");
        }

        if (ammoDisplay == null){
            Debug.LogError("ammoDisplay Text UI is not assigned in Inspector!");
        }
       
        ammoToText();                   
    }

    void ammoToText(){
        if(ammoDisplay != null){
            ammoDisplay.text = CurrentPlayerStats.ammo.ToString() + "/" + BasePlayerStats.ammo.ToString();
        }
        else{
            Debug.Log("ammoDisplay is null");
        }
    }

    void Update()
    {
        ammoToText();
    }
}
