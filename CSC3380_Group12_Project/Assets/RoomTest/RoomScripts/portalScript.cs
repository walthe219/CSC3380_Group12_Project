using UnityEngine;

public class portalScript : MonoBehaviour
{
    
    public Transform destination;
    public float heightOffset = 1;
    public float distanceOffset = 1;

    private float timer = 9999;
    public float waitTime = 3;
    private newMoveScript move  = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            move = other.GetComponent<newMoveScript>();
            move.enabled = false;

            other.transform.position = destination.position + transform.forward*distanceOffset + transform.up * heightOffset;
            other.transform.rotation = destination.rotation;

            Debug.Log("Teleported to " + destination.position);
            timer = 0;
           
        }
    }

    private void Update()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (move != null)
            {
                Debug.Log("Reenabled");
                move.enabled = true;
                move = null;
            }
            
        }
    }
}
