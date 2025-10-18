using UnityEngine;
using System;

#pragma warning restore 0414
public class UnlockScript : MonoBehaviour
{
    //This script will get more upgrades form an array

    //Dont keep the following declarations:
       public bool HealthInc;
        public bool hasDash;
        public bool hasDJ;
        public bool StaminaInc;
        public bool MagSizeInc;
    
    public enum UpgradeType { Dash, HealthUp, StamUp, MagUp, DoubJump }

    //public delegate void unlockMngr(string upgrade); not in use
    //public static event unlockMngr unlockD; not in use
    public static event Action<UpgradeType, bool> unlocker;

    //We can use this delegate to subscribe different functions in other classes
    //For example, in the movement script lets say there is a dash function that has bool = false, we can create a function that subscribes to the unlock event
    //and in that functions code block we can set if(upgrade == dash) then set dash = true and in the update code block we can have if (dash is able to be unlcoked && keyIsPressed)
    //then upgradeData.unlock("Dash") and when taht event is triggered the subscriber function will set dash = true and now we are able to use the dash function by wrapping
    //the dash function in an if statement that checks if(dahs == true && isKeyPressed){Dash();}



    public void unlock(UpgradeType upgrade, bool ToF){
      
        //Dont keep this block of code in this function just keep the invoke
     
        
        switch(upgrade){
            case UpgradeType.HealthUp:
                HealthInc = ToF;
                break;
            case UpgradeType.Dash:
                hasDash = ToF;
                break;
            case UpgradeType.DoubJump:
                hasDJ = ToF;
                break;
            case UpgradeType.StamUp:
                StaminaInc = ToF;
                break;
            case UpgradeType.MagUp:
                MagSizeInc = ToF;
                break;
            default:
                Debug.LogWarning(upgrade + " is not a valid upgrade.");
                break;
        }
        unlocker?.Invoke(upgrade, ToF);
    }

}

//Guide on events
//In movement script declare a bool for each movement related bool
//In the subscriber function you can do the body of this unlock function, the unlock function really only needs unlocker?.invoke(upgrade, ToF)
//Then in update you can do if(criteria met){unlocker.unlock(Dash, true)}
    //This will send the event information to the functions that are subscribed to the event
        //Then the subscription function will set the bool in the movement script to true
            //Then you can write in update if(hasDash && keypressed){Dash();}
//For the future all the subscriber functions can follow the above code's format (the case switching and setting to true)

//And for HealthUp and stat increases we can have a serialized field for PlayerStats and one for UpgradeData1 and apply UpgradeData1.exampleDeltaStat += PlayerStats.exampleStat;
    //*Or maybe try to use events
        //You can use an event in UpgradeData that passes through exampleDelta and then player stat's subscriber function adds it to currentHealth


#pragma warning restore 0414