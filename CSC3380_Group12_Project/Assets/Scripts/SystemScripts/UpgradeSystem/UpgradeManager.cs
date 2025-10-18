using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
        
    [SerializeField] List<Upgrade> acquiredUpgrades = new List<Upgrade>();
    [SerializeField] PlayerStats DefaultStats; //Stats used to reset other PlayerStats at start of Game, should never change during the game
    [SerializeField] PlayerStats BaseStats; //Base value for stats, ie max values, can change in the game w upgrades or status effects
    [SerializeField] PlayerStats CurrentStats; //Current values for stats, ex. current health of speed, effected by indivual actions

    [SerializeField] UpgradeSpace currentUpgradeSpace;


    private void Start()
    {
        currentUpgradeSpace = new UpgradeSpace(); //initialize the UpgradeSpace

        //Reset PlayerStats SOs to default values
        BaseStats.set(DefaultStats);
        CurrentStats.set(DefaultStats);
    }
    public void applyUpgrade(UpgradeData upgrade)
    {
        BaseStats.add(upgrade);
        CurrentStats.add(upgrade);
        upgrade.activate();
    }

        
    public void addUpgrade(Upgrade upgrade)
    {
        Debug.Log("Adding Upgrade " +  upgrade.data.ID);
        acquiredUpgrades.Add(upgrade);
        applyUpgrade(upgrade.data);

    }

    [ContextMenu("addUpgrade()")]
    public void addUpgrade()
    {
        Upgrade u = new Upgrade(currentUpgradeSpace.pullUpgrade());
        addUpgrade(u);
    }

    public void removeUpgrade(Upgrade upgrade){}

}
