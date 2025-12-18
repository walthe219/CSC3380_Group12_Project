using UnityEngine;

public class GunLifeSteal : MonoBehaviour
{
    [SerializeField] bool LifeStealUnlocked = false;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;

    private void Start()
    {
        GunScript.OnTargetHit += gainHealth;
        GunScript.OnMiss += loseHealth;
        GunScript.OnNonTargetHit += loseHealth;

        UnlockFunctions.UnlockLifeStealEvent += unlockLifeSteal;
    }

    [ContextMenu("unlockLifeSteal")]
    public void unlockLifeSteal()
    {
        LifeStealUnlocked = true;
    }

    void gainHealth(RaycastHit NOTUSED)
    {
        if (LifeStealUnlocked && currPlayerStats.health < BasePlayerStats.health)
        {
            currPlayerStats.health = (float)(currPlayerStats.health + (currPlayerStats.damage * 0.10)); //If LifeSteal Upgrade is unlocked, then whena player successfully
                                                                                                        //hits an enemy they gain a percentage of the damage they deal to their health
                                                                                                        //Otherwise, if they miss they lose that percentage of health
        }
    }

    void loseHealth(RaycastHit NOTUSED)
    {
        if (LifeStealUnlocked && currPlayerStats.health < BasePlayerStats.health)
        {
            currPlayerStats.health = (float)(currPlayerStats.health - (currPlayerStats.damage * 0.10)); //Where the player loses the percentage of health if they miss
        }
    }
}
