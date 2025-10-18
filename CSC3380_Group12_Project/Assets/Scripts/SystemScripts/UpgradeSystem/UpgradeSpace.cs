using System.Collections.Generic;
using UnityEngine;

/*
 * Data Strucutre containing all possibleUpgrades and future upgrades for the player, with only possibleUpgrades Accessible
 * 
 * Still WIP
 * for now loads all upgrades and pops then in order
 */
public class UpgradeSpace
{
    [SerializeField] List<UpgradeData> possibleUpgrades;
    public UpgradeSpace()
    {
        UpgradeData[] allUpgrades = Resources.LoadAll<UpgradeData>("UpgradeData");
        possibleUpgrades = new List<UpgradeData>(allUpgrades);
        string s = string.Join(", ", possibleUpgrades);
        Debug.Log("possibleUpgrades: " + s);
    }
    public UpgradeData pullUpgrade()
    {
        UpgradeData u = possibleUpgrades[0];
        possibleUpgrades.RemoveAt(0);
        return u;
    }


}
