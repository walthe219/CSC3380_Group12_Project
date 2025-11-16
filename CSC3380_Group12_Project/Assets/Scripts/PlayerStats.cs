using UnityEngine;

/*
 * Represents all the players stats, inherits upgradable stat fields from UpgradeableStatContainer
 * ONLY NONUPGRADEABLE STAT fields add here
 */
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats", order=1)]
public class PlayerStats : UpgradeableStatContainer
{
    public int numRoomsComp;
<<<<<<< HEAD
    public int gunRange;
=======


>>>>>>> origin/Josh-improved-UpgradeSpace
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
