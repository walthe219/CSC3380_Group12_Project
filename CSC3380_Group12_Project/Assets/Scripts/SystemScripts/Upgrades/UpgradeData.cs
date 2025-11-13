using UnityEngine;
using System;
using System.Text;
using System.Reflection.Emit;

/*
 * Contains all the data for an upgrade, inherits upgradeable stat fields from UpgradeableStatContainer
 * To create new data for an upgrade:
 *  Go to Assets/Resources/UpgradeData, then in the folder click Create/ScriptableObject/UpgradeData
 *  Then define that stats for that object
 */
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData", order =1)]
public class UpgradeData : UpgradeableStatContainer
{
    public enum Rarity
    {
        COMMON, RARE, EPIC
    }

    [Tooltip("Name used to represent the upgade in game and Console")]
    public string ID;

    [Tooltip("Rarity tier, affects how common this upgrade is to appear")]
    public Rarity rarity;

    [Tooltip("If repeateable, then upgrade can be earned infinite times")]
    public bool isRepeatable;

    [Tooltip("List of upgrades requried before this upgrade can be acquired")]
    public UpgradeData[] prerequisites;

    [Tooltip("List of upgrades that cannot be had at the same time of this upgrade")]
    public UpgradeData[] mutuallyExclusives;

    [Tooltip("List of unlocks acquired from this upgrade")]
    public UnlockFunctions.Unlockable[] unlocks;

    [Tooltip("Description of upgrades affects")]
    public string description;

    //Create methods to check fields are correct, no duplicates, mutually exlusive listed on both upgrades, self not preq or mutually exclusive, repeateable cannot have dependencies or mutuallly exclusive,
    //remove null values in arrays
    void Awake()
    {

    }
    public override string ToString()
    {
        return ID;
    }


    // ================ PRINT METHODS ================

    
    //returns string "UpgradeName [RARITY][REPEATABLE]" 
    string printLabel()
    {
        StringBuilder printout = new StringBuilder();
        printout.Append($"[{rarity}]");
        if (isRepeatable) printout.Append("[REPEATABLE]");

        return printout.ToString();
    }

    //returns string "Unlocks: Unlock0,Unlock1, ....., UnlockN" 
    string printUnlocks()
    {
        string[] list = Array.ConvertAll(unlocks, element => element.ToString());
        return $"Unlocks: {string.Join(",",list)}";
    }

    /*
     * returns full upgrade description string
     * C
     * 
     * UpgradeName [RARITY][REPEATABLE]:
     * Stat0 = value
     * Stat1 = value
     * .....
     * StatN = value
     * Unlocks: Unlock0,Unlock1, ....., UnlockN
     * Description:
     * Upgrade description text goes here........
     */
    public string printDescription(bool ID=false, bool label = true, bool stats = true, bool unlock = true, bool descr = true)
    {
        //Name and Rarity
        StringBuilder printout = new StringBuilder();
        if (ID) printout.Append(ID);
        if(label) printout.AppendLine(printLabel());

        //Stat changes if they exist
        if (stats)
        {
            string statPrint = printStats();
            if (statPrint.Length > 0)
            {
                //printout.AppendLine("Stats:");
                printout.AppendLine(statPrint);
            }
        }
        //Unlocks if they exist
        if (unlock)
        {
            if (unlocks != null && unlocks.Length > 0)
            {
                printout.AppendLine(printUnlocks());
            }
        }
        //Description if it exists
        if (descr)
        {
            if (description.Trim().Length > 0)
            {
                printout.AppendLine($"Description:");
                printout.Append(description);
            }
        }
        return printout.ToString();

    }
}
