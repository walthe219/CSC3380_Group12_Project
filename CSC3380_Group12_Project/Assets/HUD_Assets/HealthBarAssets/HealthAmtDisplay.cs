using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthAmtDisplay : MonoBehaviour
{
    //*************************Move this to testplayer script
    public TextMeshProUGUI HealthDisplay;
    TestPlayer p1;
    private int Health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        HealthDisplay = GameObject.Find("HealthAmtDisplay").GetComponent<TextMeshProUGUI>();
        
      

    }

   

    void healthtoText(){
         if(HealthDisplay != null){
            HealthDisplay.text = Health.ToString();
        }
        else{
            Debug.Log("Health is null");
        }
    }

    void setHealth(int Health){
        this.Health = Health;
    }

  
    // Update is called once per frame
    void Update()
    {
        healthtoText();

    }
}
