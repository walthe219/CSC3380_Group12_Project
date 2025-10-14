using UnityEngine;

public class PlayerStats : MonoBehaviour
{


    public int baseHealth = 100;
    public int baseStamina = 100;
    public int baseMagSize = 30;
    public int baseAmmo;

    public int currentHealth;
    public int currentStamina;
    public int currentMagSize;
    public int currentAmmo;
    

    public void Start(){
        currentHealth = baseHealth;
        currentStamina = baseStamina;
    }

    private void OnEnable(){
        //UnlockScript.unlockD += PlayerStatUpgrdMngr;
    }

    private void OnDisable(){
        //UnlockScript.unlockD -= PlayerStatUpgrdMngr;
    }

    private void PlayerStatUpgrdMngr(string upgrade){
        

        
    }
}
