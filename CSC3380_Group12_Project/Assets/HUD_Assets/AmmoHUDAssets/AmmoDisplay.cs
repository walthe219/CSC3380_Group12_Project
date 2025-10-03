using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AmmoDisplay : MonoBehaviour
{

    private int ammo=-1;
    private bool isFiring;
    private bool isReloading;
    public TextMeshProUGUI ammoDisplay;
    private int magSize = 10;
    private int delay_x;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setMagSize(30);
        delayTime(3);
        ammo=magSize;
        ammoDisplay = GameObject.Find("AmmoDisplay").GetComponent<TextMeshProUGUI>();
        
    }

    public int setMagSize(int magSize){
        this.magSize = magSize;
        return magSize;
    }

    void delayTime(int delay_x){
        this.delay_x = delay_x;
    }

    //Implememnt reload and shooting delay
    //Maybe couple seconds after relaod before you cans start shooting again
     IEnumerator reload(){
            isReloading = true;
            Debug.Log("Reloading......");
            yield return new WaitForSeconds(delay_x);
            ammo=magSize;
            isReloading=false;
            Debug.Log("Reloaded!");
        
    }

    

    void shoot(){
        if(Input.GetMouseButtonDown(0) && !isFiring && ammo > 0){//left click = 0
            isFiring = true;
            ammo--;
            isFiring = false;
        } 
    }

    void ammoToText(){
        if(ammoDisplay != null){
            ammoDisplay.text = ammo.ToString();
        }
        else{
            Debug.Log("ammoDisplay is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ammoToText();
        shoot();
        if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            StartCoroutine(reload());
        }
    }
}
