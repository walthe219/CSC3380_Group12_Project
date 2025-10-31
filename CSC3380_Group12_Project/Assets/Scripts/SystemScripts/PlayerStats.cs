using UnityEngine;

/*
 * Represents all the players stats, inherits stat fields for upgradable stats from StatContainer, for nonupgradable stats add here
 */
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats", order=1)]
public class PlayerStats : StatContainer
{
    public float RoomsComp;
    private void OnEnable(){
        
    }

    private void OnDisable(){
       
    }

}
