using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Codice.Client.Common.Servers.RecentlyUsedServers;

public class PillarScript : MonoBehaviour
{
    public Interact OpenFromInteraction;
    public GameObject UIContainer;
    [SerializeField] public TextMeshProUGUI HealthChange;
    [SerializeField] public TextMeshProUGUI hsubox1;
    [SerializeField] public TextMeshProUGUI hsubox2;
    [SerializeField] public TextMeshProUGUI hsubox3;
    [SerializeField] public GameObject rusure;
    [SerializeField] public GameObject buybttn0;
    [SerializeField] public GameObject buybttn1;
    [SerializeField] public GameObject buybttn2;
    [SerializeField] public TextMeshProUGUI MiddleID;
    [SerializeField] public TextMeshProUGUI LeftID;
    [SerializeField] public TextMeshProUGUI RightID;
    [SerializeField] GameObject HealthSacrificeMenu;
    [SerializeField] RoomManager RoomManager;
    [SerializeField] PlayerStats CurrentPlayerStats;
    private bool PillarMenuOpened;
    SacrificeUpgradeData[] upgradechoices;
    public event Action<Upgrade> UpgradePurchased;
    public bool q;
    private int pendingUpgradeIndex = -1;
    


    UpgradeSpace sacrificeSpace; //Create UpgradeSpace for Health Sacrifice
    

    public void Start(){
        sacrificeSpace = new UpgradeSpace(null, "HealthSacrifice"); 
        DisplaySacrificeUpgrades();
        q = false;
        
    }

    private void OnEnable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted += OpenPillarMenu;
        }

        RoomManager.RoomCleared += DisplaySacrificeUpgrades;
    }

    private void OnDisable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted -= OpenPillarMenu;
        }

        RoomManager.RoomCleared += DisplaySacrificeUpgrades;
    }

    public void OpenPillarMenu(){
        if(PillarMenuOpened == false){
            Debug.Log("Opened Pillar Menu");
            Time.timeScale = 0f;
            Cursor.visible = true;            // Hide cursor during gameplay
            Cursor.lockState = CursorLockMode.None;
            HealthSacrificeMenu.SetActive(true);
            PillarMenuOpened = true;
            UIContainer.SetActive(false);
        }
        else{
            Debug.Log("Closed Pillar Menu");
            Time.timeScale = 1f;
            Cursor.visible = false;            // Hide cursor during gameplay
            Cursor.lockState = CursorLockMode.Locked;
            HealthSacrificeMenu.SetActive(false);
            PillarMenuOpened = false;
            UIContainer.SetActive(true);
        }
    }

    public void BackBttn() {
        Debug.Log("Closed Pillar Menu");
        Time.timeScale = 1f;
        Cursor.visible = false;            // Hide cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
        HealthSacrificeMenu.SetActive(false);
        PillarMenuOpened = false;
        UIContainer.SetActive(true);
    }

    public void NoBttn()
    {
        Debug.Log("PUI is " + pendingUpgradeIndex);
        rusure.SetActive(false);
        if (pendingUpgradeIndex != -1)
        {
            CurrentPlayerStats.health = CurrentPlayerStats.health / (1 - (upgradechoices[pendingUpgradeIndex].HealthCostPercent / 100f));
        }
        if (pendingUpgradeIndex == -1)
        {
            Debug.Log("Make sure all buttons share the same instance of pillarscript!");
        }

    }

    public void YesBttn()
    {
        rusure.SetActive(false);
        Debug.Log(pendingUpgradeIndex);
        if(pendingUpgradeIndex == -1)
        {
            Debug.Log("Make sure all buttons share the same instance of pillarscript!");
        }
        if (pendingUpgradeIndex != -1)
        {
            PurchaseUpgrade(pendingUpgradeIndex);
            

            
        }
    }

    public void indexSelector(int i) {
        pendingUpgradeIndex = i;
        Debug.Log("index selector called with i = " + i);
        Debug.Log("Health Cost Percentage: " + upgradechoices[pendingUpgradeIndex].HealthCostPercent);
        CurrentPlayerStats.health = CurrentPlayerStats.health * (1 - (upgradechoices[pendingUpgradeIndex].HealthCostPercent / 100f)); //successfully decrements health by Health Cost Percent
        Debug.Log("Health is now: " + CurrentPlayerStats.health);
        HealthChange.text = "Health after purchase: " + CurrentPlayerStats.health.ToString();
        rusure.SetActive(true);
        

        
    }

    public void PurchaseUpgrade(int i){
        //upgrade = upgradechoices[i]
        //currentstats.hp * upgrade.healthcostpercentage


        /*Debug.Log("Health Cost Percentage: " + upgradechoices[i].HealthCostPercent);
        CurrentPlayerStats.health = CurrentPlayerStats.health * (1 - (upgradechoices[i].HealthCostPercent / 100f)); //successfully decrements health by Health Cost Percent

        Debug.Log("Health is now: " + CurrentPlayerStats.health);*/
        Upgrade purchasedUpgrade = new Upgrade(upgradechoices[i]);
            UpgradePurchased?.Invoke(purchasedUpgrade);
            if (i == 0)
            {
                buybttn0.gameObject.SetActive(false);
            }
            else if (i == 1)
            {
                buybttn1.gameObject.SetActive(false);
            }
            else
            {
                buybttn2.gameObject.SetActive(false);
            }
            
        
            //TODO: Whenever buy button is pressed, add upgrade stats to basestats double check that
            //And subscribe addUpgrade to your event in the start method of UpgradeManager if u use an action

        
        

    }

    public void DisplaySacrificeUpgrades(){ //this method successfulldisplays 3 random upgrades in the menu
        UpgradeData[] sample = sacrificeSpace.samplePossibleUpgrades(3);
        upgradechoices = Array.ConvertAll(sample, element => (SacrificeUpgradeData) element);

        hsubox1.text = upgradechoices[0].printDescription(ID:false, label:false, stats:true, descr:true);
        hsubox2.text = upgradechoices[1].printDescription(ID:false, label:false, stats:true, descr:true);
        hsubox3.text = upgradechoices[2].printDescription(ID:false, label:false, stats:true, descr:true);

        LeftID.text = upgradechoices[0].ID;
        MiddleID.text = upgradechoices[1].ID;
        RightID.text = upgradechoices[2].ID;

        //non-importatn todo: see if you can separate the name of the upgrade (ID) from the description, right now it looks pushed together

        //'refresh' method
        //new sample of three upgrades and display the upgrade information in the menu
        //subscribe to event in roomgenerator where upon room completion get a new batch of sacrifice upgrades
        //display upgradechoices[0->2]
        //for each tmp i can do upgradechoice[i].printDescription(parameters blah blah blah)

    }

}

//*******NOTE************: If it looks like the upgrades are being applied via index[i+1] make sure you are using the scene cthulhu idol instance for the indexSelector methods
//in the inspector of th ebuttons instead of the TEST healthsacrificepillar

//ALSO! if the buttons seem to work and health isbeing decremented and health change is working but the upgrades seem like they are not applying at all? Go to UpgradeManager and make
//sure it is using the same instance as teh rest