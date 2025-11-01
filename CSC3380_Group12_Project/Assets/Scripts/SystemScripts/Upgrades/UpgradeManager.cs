using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
        
    
    [SerializeField] PlayerStats DefaultStats; //Stats used to reset other PlayerStats at start of Game, should never change during the game
    [SerializeField] PlayerStats BaseStats; //Base value for stats, ie max values, can change in the game w upgrades or status effects
    [SerializeField] PlayerStats CurrentStats; //Current values for stats, ex. current health of speed, effected by indivual actions

    UpgradeSpace currentUpgradeSpace;
    List<Upgrade> acquiredUpgrades = new List<Upgrade>();

    public static UpgradeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads
        }
    }

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


    

    void Start()
    {
        currentUpgradeSpace = new UpgradeSpace(); //initialize the UpgradeSpace
        RoomManager.Instance.RecieveReward += applyReward;

        //Reset PlayerStats SOs to default values
        BaseStats.set(DefaultStats);
        CurrentStats.set(DefaultStats);
    }

    void applyReward(string ID)
    {
        Upgrade u = new Upgrade(currentUpgradeSpace.pullUpgrade(ID));
        addUpgrade(u);

    }

    void addUpgrade(Upgrade upgrade)
    {
        Debug.Log("Adding Upgrade " + upgrade.data.ID);
        acquiredUpgrades.Add(upgrade);
        applyUpgrade(upgrade);

    }

    void applyUpgrade(Upgrade upgrade)
    {
        BaseStats.add(upgrade.data);
        CurrentStats.add(upgrade.data);
        upgrade.activate();

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

    //See UpgradeSpace.samplePossibleUpgrades()
    public UpgradeData[] samplePossibleUpgrades(int num)
    {
        return currentUpgradeSpace.samplePossibleUpgrades(num);
    }

}
