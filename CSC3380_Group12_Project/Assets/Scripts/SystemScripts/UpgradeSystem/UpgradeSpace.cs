using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

/*
 * Data strucutre containing all possibleUpgrades and future upgrades for the player, only possibleUpgrades can be retrieved
 * 
 * Still WIP
 * for now loads all upgrades and pops randomly, removes item on pop if not repeatable upgrade
 */
public class UpgradeSpace
{
    [SerializeField] List<UpgradeData> possibleUpgrades;

    public UpgradeSpace(UpgradeData[] allUpgrades = null)
    {
        if (allUpgrades == null)
        { 
            allUpgrades = Resources.LoadAll<UpgradeData>("UpgradeData");
        }
        possibleUpgrades = new List<UpgradeData>(allUpgrades);
        print();
    }

    //Pulls random upgrade and removes it from possibleUpgrades
    public UpgradeData pullUpgrade()
    {
        int rand = Random.Range(0, possibleUpgrades.Count);
        UpgradeData u = possibleUpgrades[rand];
        pullUpgrade(u);
        return u;
    }

    //finds upgrade if it is in possibleUpgrades and returns it, else returns null. Upgrade is not removed
    public UpgradeData findUpgrade(string ID)
    {
        foreach (UpgradeData u in possibleUpgrades)
        {
            if (u.ID.Equals(ID))
            {
                return u;
            }
        }
        Debug.LogError($"Could not find upgrade with ID {ID} in possibleUpgrades");
        return null;
    }

    //Pulls upgrade with given ID and removes it from possibleUpgrades if it exists, else returns null
    public UpgradeData pullUpgrade(string ID)
    {
        UpgradeData u = findUpgrade(ID);
        if (u != null && !u.isRepeatable) { 
            possibleUpgrades.Remove(u);
        }
        return u;
    }

    //Pulls  given upgrade and removes it from possibleUpgrades if it exists, else returns null
    public UpgradeData pullUpgrade(UpgradeData u)
    {
        return pullUpgrade(u.ID);
    }
    
    // returns an array of random upgrades in possibleUpgrades that can be used by other classes without affecting the actual UpgradeSpace
    // Changes to this array will not change possibleUpgrades
    public UpgradeData[] samplePossibleUpgrades(int num)
    {
        UpgradeData[] temp = possibleUpgrades.ToArray();
        temp = (UpgradeData[])ArrayHelper.Shuffle(temp);
        UpgradeData[] sample = new UpgradeData[num];
        for (int i = 0; i < num; i++)
        {
            sample[i] = temp[i];
        }

        return sample;
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
