using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class PillarScript : MonoBehaviour
{
    public Interact OpenFromInteraction;
    [SerializeField] public TextMeshProUGUI hsubox1;
    [SerializeField] public TextMeshProUGUI hsubox2;
    [SerializeField] public TextMeshProUGUI hsubox3;
    [SerializeField] public TextMeshProUGUI MiddleID;
    [SerializeField] public TextMeshProUGUI LeftID;
    [SerializeField] public TextMeshProUGUI RightID;
    [SerializeField] GameObject HealthSacrificeMenu;
    [SerializeField] PlayerStats CurrentPlayerStats;
    private bool PillarMenuOpened;
    SacrificeUpgradeData[] upgradechoices;
    public event Action<UpgradeData> UpgradePurchased; // instead of using action use unity event, and then subscribe UpgradePurchased to UpgradeManager.addUpgrade


    UpgradeSpace sacrificeSpace; //Create UpgradeSpace for Health Sacrifice
    

    public void Start(){
        sacrificeSpace = new UpgradeSpace(null, "UpgradeData/HealthSacrifice"); //Note: should work after i merge with josh's branch but wait until further upgradespace testing
        DisplaySacrificeUpgrades();
        
        
    }

    private void OnEnable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted += OpenPillarMenu;
        }
    }

    private void OnDisable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted -= OpenPillarMenu;
        }
    }

    public void OpenPillarMenu(){
        if(PillarMenuOpened == false){
            Debug.Log("Opened Pillar Menu");
            Time.timeScale = 0f;
            Cursor.visible = true;            // Hide cursor during gameplay
            Cursor.lockState = CursorLockMode.None;
            HealthSacrificeMenu.SetActive(true);
            PillarMenuOpened = true;
        }
        else{
            Debug.Log("Closed Pillar Menu");
            Time.timeScale = 1f;
            Cursor.visible = false;            // Hide cursor during gameplay
            Cursor.lockState = CursorLockMode.Locked;
            HealthSacrificeMenu.SetActive(false);
            PillarMenuOpened = false;
        }
    }

    public void PurchaseUpgrade(int i){
        //upgrade = upgradechoices[i]
        //currentstats.hp * upgrade.healthcostpercentage
        Debug.Log("Health Cost Percentage: " + upgradechoices[i].HealthCostPercent);
        CurrentPlayerStats.health = CurrentPlayerStats.health * (1 - (upgradechoices[i].HealthCostPercent/100f)); //successfully decrements health by Health Cost Percent
        Debug.Log("Health is now: " + CurrentPlayerStats.health);

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
