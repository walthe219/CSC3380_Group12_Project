using System;
using UnityEngine;

/*
 * Teleports a object tagged player on contact with portal, 
 * disables player movement and look scripts so that position and rotatation of player can be set by this script
 */
public class portalScript : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] Transform destination; //where to teleport, usally another portal
    [SerializeField] float heightOffset = 1; // distance above destination to teleport
    [SerializeField] float distanceOffset = 2f; //distance in front of destination
    [SerializeField] GameObject portalFrame;
    [SerializeField] Collider portalCollider;

    static MonoBehaviour movementScript  = null;
    static MonoBehaviour lookScript = null;
    static bool teleportOnCoolDown = false;
    static float staticTimerStart = float.NaN;
    static float teleportCoolDownDuration = 3f;
    static float scriptDisableDuration = 0.05f; //how long to disable move and lookscript, if too short then player position will not 

    public event Action<GameObject> PlayerEnterPortal;
    //public event Action<GameObject> PlayerArrivePortal;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.tag.Equals("Player"))
        {
            if (teleportOnCoolDown)
            {
                Debug.Log("Cannot teleport yet");
                return;
            }

            //Get control scripts so that they can be disabled
            movementScript = other.GetComponent<newMoveScript>();
            if (movementScript == null)
            {
                Debug.LogError("Change move script name in portalScript code GetComponent<SCRIPT NAME HERE>");
                return;
            }
            lookScript = other.GetComponent<cameraScript>();
            if (lookScript == null)
            {
                Debug.LogError("Change look script name in portalScript code GetComponent<SCRIPT NAME HERE>");
                return;
            }


            //Disable controller scripts so teleport is not overwritten by these scripts
            movementScript.enabled = false;
            lookScript.enabled = false;

            //Teleport player
            other.transform.position = destination.position + destination.forward * distanceOffset + destination.up * heightOffset;
            other.transform.rotation = destination.rotation;
            Debug.Log("Teleported to " + destination.position + " with Rotation " + destination.rotation+ $"\nTeleport on cooldown for {teleportCoolDownDuration} seconds".ToUpper());
            
            //Begin timer to reenable script and until player can teleport again
            staticTimerStart = Time.time;
            teleportOnCoolDown = true;
            PlayerEnterPortal?.Invoke(this.gameObject);
            //Debug.Log("Start Time: " + staticTimerStart);

            
        }
        else Debug.LogError("Object needs 'Player' tag to use portal");
    }

    public void ActivatePortal() 
    {
        portalFrame.SetActive(true);
        portalCollider.enabled = true;
    }
    public void DeactivatePortal() 
    {
        portalFrame.SetActive(false);
        portalCollider.enabled = false;
        
    }

    private void Update()
    {
        if(Time.time - staticTimerStart > scriptDisableDuration)
        {
            if (movementScript != null)
            {
                Debug.Log("Controls Reenabled");
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
        ActivatePortal();
        
        otherScript.setDestination(this.transform);
        otherScript.ActivatePortal();


        //PlayerEnterPortal += otherScript.PlayerArrivePortal;          OnArrive could possibly be useful, but this implementation is terrible idea, maybe can find a solution
        //otherScript.PlayerEnterPortal+= PlayerArrivePortal;
    }

    public void setDestination(Transform pos)
    {
        destination = pos;
    }

    private void OnDestroy()
    {
        if (movementScript != null || lookScript != null)
        {
            //movementScript.enabled = true;
            //lookScript.enabled = true;
        }
    }
}
