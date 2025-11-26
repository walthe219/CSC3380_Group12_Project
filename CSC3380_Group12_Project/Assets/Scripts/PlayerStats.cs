using UnityEngine;

/*
 * Represents all the players stats, inherits stat fields for upgradable stats from UpgradeableStatContainer, for nonupgradable stats add here
 */
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats", order=1)]
public class PlayerStats : UpgradeableStatContainer
{
    public int numRoomsComp;
    
    private void OnEnable(){
        
    }

    private void OnDisable(){
       
    }

    public override string printAllStats()
    {
        string printout = base.printAllStats();
        return  printout + $"\nRooms Cleared: {numRoomsComp}";
    }

}
