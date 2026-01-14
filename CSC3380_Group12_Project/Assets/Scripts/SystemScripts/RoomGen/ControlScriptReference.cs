using System;
using UnityEngine;



//Attacht this script to the object holding the Player's collider
public class ControlScriptReference : MonoBehaviour
{
    [SerializeField] MonoBehaviour moveScript;
    [SerializeField] MonoBehaviour lookScript;
    public GameObject ParentObject;

    //When this script is disabled or enabled by portalScript, call event for Control scripts to do the same
    private void OnDisable()
    {
        if (moveScript != null )moveScript.enabled = false;
        if (lookScript != null) lookScript.enabled = false;
    }

    private void OnEnable()
    {
        if (moveScript != null) moveScript.enabled = true;
        if (lookScript != null) lookScript.enabled = true;
    }
}
