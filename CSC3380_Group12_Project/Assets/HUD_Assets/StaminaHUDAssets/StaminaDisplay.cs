using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StaminaDisplay : MonoBehaviour
{
    
    public int stamina;
    public TextMeshProUGUI staminaText;
    private int delay_x;
    private int maxStamina;
    private bool isCoroutineRunning = false;
    private int deplAmtl;
    private int regenAmt;

    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats DefaultStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not assigned in insepctor (StaminaDisplay)");
        }

        delayTimeStam(2);
        CurrentPlayerStats.stamina = DefaultStats.stamina;
        staminaText = GameObject.Find("StamDisplay").GetComponent<TextMeshProUGUI>();

    }

    void stamToText(){
         if(staminaText != null){
            staminaText.text = CurrentPlayerStats.stamina.ToString();
        }
        else{
            Debug.Log("StamDisplay is null");
        }
    }

    void setMaxStamina(int maxStamina){
        this.maxStamina = maxStamina;
    }

    void deplete(int deplAmt){
        if(CurrentPlayerStats.stamina > 0){
        CurrentPlayerStats.stamina = CurrentPlayerStats.stamina - deplAmt;
        //Debug.Log("depleting");
        }
        
    }

    void delayTimeStam(int delay_x){
        this.delay_x = delay_x;
    }

    //IEnumerator regenerate(int regenAmt){
    //    if(stamina < 100){
    //        isCoroutineRunning=true;
    //        CurrentPlayerStats.stamina = CurrentPlayerStats.stamina+regenAmt;
    //        yield return new WaitForSeconds(delay_x);
    //        //Debug.Log("regenerating");
    //        isCoroutineRunning=false;
    //    }
    //    
    //}

    // Update is called once per frame
    void Update()
    {
        stamToText();
        
    }
}
