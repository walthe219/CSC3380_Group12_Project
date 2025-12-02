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
    [SerializeField] float distanceOffset = 2.5f; //distance in front of destination
    [SerializeField] GameObject portalFrame; //Game object representing the teleporter ring/frame
    [SerializeField] Collider portalCollider;
    [SerializeField] Collider portalHitboxCollider;
    public float portalHealth = 50f;

    static ControlScriptReference reference = null;
    static bool teleportOnCoolDown = false;
    static float staticTimerStart = float.NaN;
    static float teleportCoolDownDuration = 3f;
    static float scriptDisableDuration = 0.05f; //how long to disable move and lookscript, if too short then player position will not 

    public event Action<GameObject> PlayerEnterPortal;
    //public event Action<GameObject> PlayerArrivePortal;
    public void setColliders(GameObject frame, Collider teleportCollider, Collider healthCollider)
    {
        portalFrame = frame;
        portalCollider = teleportCollider;
        portalHitboxCollider = healthCollider;
    }
    private void Awake()
    {
        if (destination == null)
        {
            DeactivatePortal();
        }
    }
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
            reference = other.GetComponent<ControlScriptReference>();
            if (reference == null)
            {
                Debug.LogError("Collider object needs ControlScriptReference script attached");
                return;
            }

            //Disable controller scripts so teleport is not overwritten by these scripts
            reference.enabled = false;

            //Teleport player
            GameObject objToTeleport = reference.ParentObject;
            objToTeleport.transform.position = destination.position + destination.forward * distanceOffset + destination.up * heightOffset;
            objToTeleport.transform.rotation = destination.rotation;
            Debug.Log("Teleported to " + destination.position + " with Rotation " + destination.rotation+ $"\nTeleport on cooldown for {teleportCoolDownDuration} seconds".ToUpper());
            
            //Begin timer to reenable script and until player can teleport again
            staticTimerStart = Time.time;
            teleportOnCoolDown = true;
            PlayerEnterPortal?.Invoke(this.gameObject);
            //Debug.Log("Start Time: " + staticTimerStart);

            
        }
        else Debug.LogWarning("Object needs 'Player' tag to use portal");
    }

    [ContextMenu("ActivatePortal()")]
    public void ActivatePortal() 
    {
        portalFrame.SetActive(true);
        portalCollider.enabled = true;
        portalHitboxCollider.enabled = false;
    }

    [ContextMenu("DeactivatePortal()")]
    public void DeactivatePortal() 
    {
        portalFrame.SetActive(false);
        portalCollider.enabled = false;
        portalHitboxCollider.enabled = true;
    }

    private void Update()
    {
        if(Time.time - staticTimerStart > scriptDisableDuration)
        {
            if (reference != null)
            {
                Debug.Log("Controls Reenabled");
                reference.enabled = true;
                reference = null;
                //movementScript.enabled = true;
                //movementScript = null;
                //lookScript.enabled = true;
                //lookScript = null;
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

        if (portalHitboxCollider.enabled == true)
        {
            if (portalHealth <= 0)
            {
                Debug.Log("The portal has been Destroyed!");
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

        //PlayerEnterPortal += otherScript.PlayerArrivePortal;  //OnArrive could possibly be useful, but this implementation is terrible idea, maybe can find a solution
        //otherScript.PlayerEnterPortal+= PlayerArrivePortal;
    }

    public void setDestination(Transform pos)
    {
        destination = pos;
        ActivatePortal();
    }
}
