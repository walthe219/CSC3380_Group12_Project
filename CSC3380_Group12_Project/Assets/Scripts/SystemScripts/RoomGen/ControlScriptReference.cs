using System;
using UnityEngine;



/*
 * Attach this script to object with Players Collider
 * Used by portalScript to referernce the Player parent object, and the players movement and look scripts
 * Parent is used to teleport the player
 * Control scripts are enabled and disabled, so the teleport is not overwritten, through events calls
*/
public class ControlScriptReference : MonoBehaviour
{
    [SerializeField] MonoBehaviour moveScript;
    [SerializeField] MonoBehaviour lookScript;
    public GameObject ParentObject;


    //When this script is disabled or enabled by portalScript, call event for Control scripts to do the same
    private void OnDisable()
    {
        moveScript.enabled = false;
        lookScript.enabled = false;
    }
    private void OnEnable()
    {
        moveScript.enabled = true;
        lookScript.enabled = true;
    }
}
