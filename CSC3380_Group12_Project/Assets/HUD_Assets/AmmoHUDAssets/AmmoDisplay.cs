using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{

    public int ammo=-1;
    public bool isFiring;
    public TextMeshProUGUI ammoDisplay;
    public int magSize = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Implememnt reload and shooting delay
    //Maybe couple seconds after relaod before you cans start shooting again
    void reload(){
        if(Input.GetKeyDown(KeyCode.R)){
            ammo=magSize;
        }
    }

    void shoot(){
        if(Input.GetMouseButtonDown(0) && !isFiring && ammo > 0){//left click = 0
            isFiring = true;
            ammo--;
            isFiring = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = ammo.ToString();
        shoot();
        reload();
    
    }
}
