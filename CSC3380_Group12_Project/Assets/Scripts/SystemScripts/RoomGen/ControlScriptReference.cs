using System;
using UnityEngine;



//Attacht this script to the object holding the Player's collider
public class ControlScriptReference : MonoBehaviour
{
    [SerializeField] MonoBehaviour moveScript;
    [SerializeField] MonoBehaviour lookScript;
    public GameObject ParentObject;


    public static event Action ScriptsDisabled;
    public static event Action ScriptsEnabled;

    private void OnDisable()
    {
        ScriptsDisabled?.Invoke();
    }

    private void OnEnable()
    {
        ScriptsEnabled?.Invoke();
    }
}
