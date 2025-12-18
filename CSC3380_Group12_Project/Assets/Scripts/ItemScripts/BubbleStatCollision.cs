using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleStatCollision : MonoBehaviour
{
    public enum BubbleType { Health, Mana, Ammo, Stamina, Upgrade }
    public BubbleType type;

    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] float statChange;
    [SerializeField] UpgradeData upgrade;
    public AudioClip bubbleSound;
    

    private void OnTriggerEnter(Collider other){
        BubbleStatsInventory bubbleinv = other.GetComponent<BubbleStatsInventory>();//other refers to the player

        //find bubbletype 

        // if found check type
        if(type != null){//test to see if i can check the type of bubble and tested it by changing the bubble type to mana and it works as intended
            gameObject.SetActive(false); //Deletes the object
            SoundFXManager.instance.PlaySoundFXClip(bubbleSound, transform, 1f);

            switch (type)
            {
                case BubbleType.Health:
                    Debug.Log("Health Bubble collected!");
                    CurrentPlayerStats.health += statChange;
                    break;

                case BubbleType.Ammo:
                    Debug.Log("Ammo Bubble collected!");
                    CurrentPlayerStats.ammo += statChange;
                    break;
                case BubbleType.Stamina:
                    Debug.Log("Stamina Bubble collected!");
                    CurrentPlayerStats.stamina += statChange;
                    break;
                case BubbleType.Upgrade:
                    break;
            }

            

        }

        if(bubbleinv != null){
            Debug.Log("Triggered");
            bubbleinv.BubbleStatCollected();
            gameObject.SetActive(false);
        }
    }

}

