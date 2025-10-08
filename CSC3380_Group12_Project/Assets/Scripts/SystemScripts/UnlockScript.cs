using UnityEngine;
#pragma warning restore 0414
public class UnlockScript : MonoBehaviour
{
    //This script will get more upgrades form an array
    
  
    public delegate void unlockMngr(string upgrade);
    public static event unlockMngr unlockD;

    //We can use this delegate to subscribe different functions in other classes
    //For example, in the movement script lets say there is a dash function that has bool = false, we can create a function that subscribes to the unlock event
    //and in that functions code block we can set if(upgrade == dash) then set dash = true and in the update code block we can have if (dash is able to be unlcoked && keyIsPressed)
    //then upgradeData.unlock("Dash") and when taht event is triggered the subscriber function will set dash = true and now we are able to use the dash function by wrapping
    //the dash function in an if statement that checks if(dahs == true && isKeyPressed){Dash();}

    public void unlock(string upgrade){
        bool HealthInc = false;
        bool StaminaInc = false;
        bool MagSizeInc = false;
  
        
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
        unlockD?.Invoke(upgrade);
    }

}
#pragma warning restore 0414