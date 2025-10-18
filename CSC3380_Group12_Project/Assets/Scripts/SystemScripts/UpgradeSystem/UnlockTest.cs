using System.Collections;
using UnityEngine;


//tests unlock functionality of UpgradeData and UnlockFunctions
public class UnlockTest : MonoBehaviour
{
    private void Start()
    {
        UnlockFunctions.Dash += unlockDash;
        Debug.Log("Subscribed to Dash");
    }

    public void unlockDash() 
    {
        Debug.Log("Unlocked Dash!");
    }
}