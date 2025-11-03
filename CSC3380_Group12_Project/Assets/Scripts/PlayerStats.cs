using UnityEngine;

/*
 * Represents all the players stats, inherits upgradable stat fields from UpgradeableStatContainer
 * ONLY NONUPGRADEABLE STAT fields add here
 */
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats", order=1)]
public class PlayerStats : UpgradeableStatContainer
{
    public int numRoomsComp;


    private void OnEnable(){
        
    }

    private void OnDisable(){
       
    }

}
