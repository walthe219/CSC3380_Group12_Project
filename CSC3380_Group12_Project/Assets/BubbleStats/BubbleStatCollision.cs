using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleStatCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other){
        BubbleStatsInventory bubbleinv = other.GetComponent<BubbleStatsInventory>();

        if(bubbleinv != null){
            Debug.Log("Triggered");
            bubbleinv.BubbleStatCollected();
            gameObject.SetActive(false);
        }
    }

}
