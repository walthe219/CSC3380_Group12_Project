using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
        
    [SerializeField] List<Upgrade> acquiredUpgrades = new List<Upgrade>();
    [SerializeField] PlayerStats DefaultStats; //Stats used to reset other PlayerStats at start of Game, should never change during the game
    [SerializeField] PlayerStats BaseStats; //Base value for stats, ie max values, can change in the game w upgrades or status effects
    [SerializeField] PlayerStats CurrentStats; //Current values for stats, ex. current health of speed, effected by indivual actions

    [SerializeField] UpgradeSpace currentUpgradeSpace;
    public static UpgradeManager instance;

    void Awake(){
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate UpgradeManager found — destroying this one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}


    

    private void Start()
    {
        currentUpgradeSpace = new UpgradeSpace(); //initialize the UpgradeSpace

        //Reset PlayerStats SOs to default values
        BaseStats.set(DefaultStats);
        CurrentStats.set(DefaultStats);
    }
    public void applyUpgrade(Upgrade upgrade)
    {
        BaseStats.add(upgrade.data);
        CurrentStats.add(upgrade.data);
        upgrade.activate();

    }

        
    public void addUpgrade(Upgrade upgrade)
    {
        Debug.Log("Adding Upgrade " +  upgrade.data.ID);
        acquiredUpgrades.Add(upgrade);
        applyUpgrade(upgrade);

    }

    public List<Upgrade> GetAcquiredUpgrades()
    {
        return acquiredUpgrades;
    }

    [ContextMenu("addUpgrade()")]
    public void addUpgrade()
    {
        Upgrade u = new Upgrade(currentUpgradeSpace.pullUpgrade());
        addUpgrade(u);
        currentUpgradeSpace.print();
    }

    public void removeUpgrade(Upgrade upgrade){}

}
