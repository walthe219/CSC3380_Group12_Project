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

        gameObject.SetActive(false); 
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

}

