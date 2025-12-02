using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AmmoDisplay : MonoBehaviour
{

    
    private bool isFiring;
    private bool isReloading;
    public TextMeshProUGUI ammoDisplay;
    private int magSize;
    private int delay_x;
    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats DefaultStats;
    [SerializeField] PlayerStats BasePlayerStats;
    //public PauseMenu1 pm;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        CurrentPlayerStats.ammo = DefaultStats.ammo;
        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not assigned in insepctor (AmmoDisplay)");
        }

        if (ammoDisplay == null){
            Debug.LogError("ammoDisplay Text UI is not assigned in Inspector!");
        }
        
        
        
        ammoToText();                   
     
        delayTime(3);
    }

    public void setMagSize(int magSize){
        this.magSize = magSize;
        if (CurrentPlayerStats != null)
            CurrentPlayerStats.ammo = magSize;

        ammoToText();
    }

    void delayTime(int delay_x){
        this.delay_x = delay_x;
    }

    //Implememnt reload and shooting delay
    //Maybe couple seconds after relaod before you cans start shooting again
     /*IEnumerator reload(){
            isReloading = true;
            //Debug.Log("Reloading......");
            yield return new WaitForSeconds(delay_x);
            CurrentPlayerStats.ammo=DefaultStats.ammo;
            isReloading=false;
            //Debug.Log("Reloaded!");
        
    } Moved to GunScript */

    
    

    /*void shoot(){
        if(Input.GetMouseButtonDown(0) && !isFiring && CurrentPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0){//left click = 0
            isFiring = true;
            CurrentPlayerStats.ammo--;
            isFiring = false;
        } 
    } Moved top GunScript */

    void ammoToText(){
        if(ammoDisplay != null){
            ammoDisplay.text = CurrentPlayerStats.ammo.ToString() + "/" + BasePlayerStats.ammo.ToString();
        }
        else{
            Debug.Log("ammoDisplay is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ammoToText();
        /*if(!PauseMenu1.GameIsPaused){
        shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            StartCoroutine(reload());
        } Moved to GunScript */
    }
}
