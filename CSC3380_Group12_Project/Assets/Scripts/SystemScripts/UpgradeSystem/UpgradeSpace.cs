using System.Collections.Generic;
using UnityEngine;

/*
 * Data Strucutre containing all possibleUpgrades and future upgrades for the player, with only possibleUpgrades Accessible
 * 
 * Still WIP
 * for now loads all upgrades and pops randomly, removes item on pop if not repeatable upgrade
 */
public class UpgradeSpace
{
    [SerializeField] List<UpgradeData> possibleUpgrades;
    public UpgradeSpace()
    {
        UpgradeData[] allUpgrades = Resources.LoadAll<UpgradeData>("UpgradeData");
        possibleUpgrades = new List<UpgradeData>(allUpgrades);
        print();
    }
    public UpgradeData pullUpgrade()
    {
        int rand = Random.Range(0, possibleUpgrades.Count);
        UpgradeData u = possibleUpgrades[rand];
        if (!u.isRepeatable)
        {
            possibleUpgrades.RemoveAt(rand);
        }
        return u;
    }

    public override string ToString()
    {
        string s = string.Join(", ", possibleUpgrades);
        return s;
    }

    public void print()
    {
        Debug.Log("possibleUpgrades: " + this.ToString());
    }


}
