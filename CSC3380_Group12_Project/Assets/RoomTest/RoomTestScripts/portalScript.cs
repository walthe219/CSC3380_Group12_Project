using System;
using UnityEngine;

/*
 * Teleports a object tagged player on contact with portal, 
 * disables player movement and look scripts so that position and rotatation of player can be set by this script
 */
public class portalScript : MonoBehaviour
{
    [SerializeField] Transform destination; //where to teleport, usally another portal
    public float heightOffset = 1; // distance above destination to teleport
    public float distanceOffset = 2f; //distance in front of destination
    public float disableTime = 0.05f; //how long to disable move and lookscript, if too short then player position will not 


    [SerializeField]float portalTimer = 9999;
    private MonoBehaviour movementScript  = null;
    private MonoBehaviour lookScript = null;

    static bool teleportOnCoolDown = false;
    static float staticTimerStart = float.NaN;
    static float teleportCoolDownDuration = 3f;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.tag.Equals("Player"))
        {
            if (teleportOnCoolDown)
            {
                Debug.Log("Cannot teleport yet");
                return;
            }

            //Disable controller scripts so teleport is not overwritten
            movementScript = other.GetComponent<newMoveScript>();
            if (movementScript == null)
            {
                Debug.LogError("Change move script name in portalScript code GetComponent<SCRIPT NAME HERE>");
                return;
            }
            movementScript.enabled = false;

            lookScript = other.GetComponent<cameraScript>();
            if (lookScript == null)
            {
                Debug.LogError("Change look script name in portalScript code GetComponent<SCRIPT NAME HERE>");
                return;
            }
            lookScript.enabled = false;

            //Teleport player
            other.transform.position = destination.position + destination.forward * distanceOffset + destination.up * heightOffset;
            other.transform.rotation = destination.rotation;

            Debug.Log("Teleported to " + destination.position + " with Rotation " + destination.rotation+ $"\nTeleport on cooldown for {teleportCoolDownDuration} seconds".ToUpper());
            portalTimer = 0;
            staticTimerStart = Time.time;

            teleportOnCoolDown = true;
            
        }
        else Debug.LogError("Object needs 'Player' tag to use teleporter");
    }

    public void ActivatePortal() { }
    public void DeactivatePortal() { }

    private void Update()
    {

        if (portalTimer < disableTime)
        {
            portalTimer += Time.deltaTime;
        }
        else
        {
            if (movementScript != null)
            {
                //Debug.Log("Controls Reenabled");
                movementScript.enabled = true;
                movementScript = null;
                lookScript.enabled = true;
                lookScript = null;
            }
           
        }
        
        if(Time.time - staticTimerStart> teleportCoolDownDuration)
        {
            if (teleportOnCoolDown)
            {
                Debug.Log("Teleport off cooldown");
                teleportOnCoolDown = false;
            }

        }
    }

    /*
     * Links two portals together by setting their destinations to each other
     */
    public void LinkPortal(GameObject other)
    {
        portalScript otherScript = other.GetComponent<portalScript>();
        if (otherScript == null) {
            Debug.LogError("can only LinkPortal() with and object using portalScript");
            return;
        }
        setDestination(other.transform);
        otherScript.setDestination(this.transform);
    }

    public void setDestination(Transform pos)
    {
        destination = pos;
    }
}
