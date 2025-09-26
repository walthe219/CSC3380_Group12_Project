using System;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    
    public Transform destination;
    public float heightOffset = 1;
    public float distanceOffset = 1;

    private float timer = 9999;
    public float waitTime = 3;

    private MonoBehaviour movementScript  = null;
    private MonoBehaviour lookScript = null;

    private void OnTriggerEnter(Collider other)
    {


        if (other.tag.Equals("Player"))
        {
            movementScript = other.GetComponent<newMoveScript>();
            if (movementScript == null)
            {
                Debug.LogError("Change MovementScript in portalScript code");
            }
            movementScript.enabled = false;

            lookScript = other.GetComponent<cameraScript>();
            if (lookScript == null)
            {
                Debug.LogError("Change LookScript in portalScript code");
            }
            lookScript.enabled = false;


            other.transform.position = destination.position + destination.forward * distanceOffset + destination.up * heightOffset;
            other.transform.rotation = destination.rotation;

            Debug.Log("Teleported to " + destination.position + " with Rotation " + destination.rotation);
            timer = 0;

        }
        else Debug.Log("Object needs Player tag to use teleporter");
    }

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
