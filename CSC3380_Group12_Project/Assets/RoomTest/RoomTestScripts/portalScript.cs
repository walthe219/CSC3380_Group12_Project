using System;
using UnityEngine;

/*
 * Teleports a object tagged player on contact with portal, 
 * disables player movement and look scripts so that position and rotatation of player can be set by this script
 */
public class portalScript : MonoBehaviour
{
    public Transform destination; //where to teleport, usally another teleporter's transfrom
    public float heightOffset = 1; // distance above destination to teleport
    public float distanceOffset = 1; //distance in front of destination to teleport
    public float waitTime = 0.05f; //how long to disable move and lookscript, if too short then player position will not change


    private float timer = 9999;
    private MonoBehaviour movementScript  = null;
    private MonoBehaviour lookScript = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movementScript = other.GetComponent<newMoveScript>();
            if (movementScript == null)
            {
                Debug.LogError("Change move script name in portalScript code GetComponent<SCRIPT NAME HERE>");
            }
            movementScript.enabled = false;

            lookScript = other.GetComponent<cameraScript>();
            if (lookScript == null)
            {
                Debug.LogError("Change look script name in portalScript code GetComponent<SCRIPT NAME HERE>");
            }
            lookScript.enabled = false;


            other.transform.position = destination.position + destination.forward * distanceOffset + destination.up * heightOffset;
            other.transform.rotation = destination.rotation;

            Debug.Log("Teleported to " + destination.position + " with Rotation " + destination.rotation);
            timer = 0;

        }
        else Debug.Log("Object needs 'Player' tag to use teleporter");
    }

    public void ActivatePortal() { }
    public void DeactivatePortal() { }

    private void Update()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (movementScript != null)
            {
                Debug.Log("Reenabled");
                movementScript.enabled = true;
                movementScript = null;
                lookScript.enabled = true;
                lookScript = null;
            }
            
        }
    }
}
