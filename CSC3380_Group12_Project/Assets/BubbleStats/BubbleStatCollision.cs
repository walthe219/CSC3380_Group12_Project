using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleStatCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other){
        BubbleStatsInventory bubbleinv = other.GetComponent<BubbleStatsInventory>();

        // Try to get BubbleType on the object the collider is attached to
    BubbleType bubble = GetComponent<BubbleType>();


    // If we found it, check the type
    if(bubble != null){//test to see if i can check the type of bubble and tested it by changing the bubble type to mana and it works as intended
        if(bubble.bubbleType == BubbleType.Type.Health){
            Debug.Log("Health Bubble collected!");
            // Add health here
        }
    }

        if(bubbleinv != null){

            

            Debug.Log("Triggered");
            bubbleinv.BubbleStatCollected();
            gameObject.SetActive(false);
        }
    }

}

