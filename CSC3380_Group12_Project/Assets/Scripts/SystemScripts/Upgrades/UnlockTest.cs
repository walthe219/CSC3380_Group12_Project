using System.Collections;
using UnityEngine;


//tests unlock functionality of UpgradeData and UnlockFunctions
public class UnlockTest : MonoBehaviour
{
    private void Start()
    {
        UnlockFunctions.UnlockDashEvent += unlockDash;
        UnlockFunctions.UnlockSlideEvent += unlockSlide;
        UnlockFunctions.UnlockGrappleEvent += unlockGrapple;
        UnlockFunctions.UnlockWallRunEvent += unlockDash;
        Debug.Log("Subscribed to UnlockFunctionEvents");
    }

    public void unlockDash() 
    {
        Debug.Log("Unlocked Dash!");
    }

    public void unlockSlide()
    {
        Debug.Log("Unlocked Slide!");
    }

    public void unlockGrapple()
    {
        Debug.Log("Unlocked Grapple!");
    }

    public void unlockWallrun()
    {
        Debug.Log("Unlocked Wallrun!");
    }
}