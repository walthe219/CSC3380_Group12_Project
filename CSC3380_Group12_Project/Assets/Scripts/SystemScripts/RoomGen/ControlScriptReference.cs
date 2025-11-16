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

    //Move and Look scripts need to subscribe to these events so they know when to disable and enable themselves
    public static event Action ScriptsDisabled;
    public static event Action ScriptsEnabled;


    //When this script is disabled or enabled by portalScript, call event for Control scripts to do the same
    private void OnDisable()
    {
        ScriptsDisabled?.Invoke();
    }
    private void OnEnable()
    {
        ScriptsEnabled?.Invoke();
    }
}
