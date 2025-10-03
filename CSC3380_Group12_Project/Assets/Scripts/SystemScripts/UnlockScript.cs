using UnityEngine;

public class UnlockScript : MonoBehaviour
{
    bool HealthInc = false;
    bool StaminaInc = false;
    bool MagSizeInc = false;
  
    public void unlock(string upgrade){
        switch(upgrade){
            case "HealthInc":
                HealthInc = true;
                break;
            case "StaminaInc":
                StaminaInc = true;
                break;
            case "MagSizeInc":
                MagSizeInc = true;
                break;
            default:
                Debug.LogWarning(upgrade + " is not a valid upgrade.");
                break;
        }
    }
}
