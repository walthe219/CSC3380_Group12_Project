using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleStatCollision : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;
    

    private void OnTriggerEnter(Collider other){
        BubbleStatsInventory bubbleinv = other.GetComponent<BubbleStatsInventory>();//other refers to the player

        //find bubbletype component
        BubbleType bubble = GetComponent<BubbleType>();

        // if found check type
        if(bubble != null){//test to see if i can check the type of bubble and tested it by changing the bubble type to mana and it works as intended
            if(bubble.bubbleType == BubbleType.Type.Health){
                Debug.Log("Health Bubble collected!");
                gameObject.SetActive(false); //Deletes the object
                // Add health here
                CurrentPlayerStats.health += 10;
                //CurrentPlayerStats.health -= 200; used to test game over screen
            }

            if(bubble.bubbleType == BubbleType.Type.Ammo){
                Debug.Log("Ammo Bubble collected!");
                gameObject.SetActive(false); //Deletes the object
                // Add ammo here
                CurrentPlayerStats.ammo += 10;
            }

            if(bubble.bubbleType == BubbleType.Type.Stamina){
                Debug.Log("Stamina Bubble collected!");
                gameObject.SetActive(false); //Deletes the object
                // Add stamina here
                CurrentPlayerStats.stamina += 10;
            }

        }

        if(bubbleinv != null){
            Debug.Log("Triggered");
            bubbleinv.BubbleStatCollected();
            gameObject.SetActive(false);
        }
    }

}

