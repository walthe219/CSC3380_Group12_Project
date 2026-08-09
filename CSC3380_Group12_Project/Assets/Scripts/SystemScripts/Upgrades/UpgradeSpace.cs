using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

/*
 * Data strucutre containing all possibleUpgrades and future upgrades for the player, only possibleUpgrades can be retrieved
 * 
 * Still WIP
 * for now loads all upgrades and pops randomly, removes item on pop if not repeatable upgrade
 * or can pop a specfic upgrade give ID or upgrade
 * Can sample upgrade space, to recieve list of random possible upgrades without removing or applying them
 */
public class UpgradeSpace
{

    const string DEFAULT_PATH = "UpgradeData";

    public static Dictionary<string, UpgradeData> upgradeDict;
    List<UpgradeData> possibleUpgrades; //List of upgrades player can currently acquire
    List<UpgradeData> futureUpgrades; //List of upgrades player could acquire in future after prereqs acquired
    List<UpgradeData> unavailableUpgrades; //List of upgrades player can nolonger acquire due to mutually exclusive upgrades

    //READ ONLY public versions of the upgrade lists, mainly for testing
    public ReadOnlyCollection<UpgradeData> ROpossibleUpgrades {get { return new ReadOnlyCollection<UpgradeData>(possibleUpgrades);}}
    public ReadOnlyCollection<UpgradeData> ROfutureUpgrades { get { return new ReadOnlyCollection<UpgradeData>(futureUpgrades); } }
    public ReadOnlyCollection<UpgradeData> ROunavailableUpgrades { get { return new ReadOnlyCollection<UpgradeData>(unavailableUpgrades); } }

    public UpgradeSpace(UpgradeData[] allUpgrades = null, string FolderPath= DEFAULT_PATH)
    {
        //load upgrades from resources if needed
        if (allUpgrades == null)
        { 
            allUpgrades = Resources.LoadAll<UpgradeData>(FolderPath);
        }

        //create upgradeDictionary if not already createed
        if(upgradeDict == null)
        {
            upgradeDict = new Dictionary<string, UpgradeData>();
            Array.ForEach(Resources.LoadAll<UpgradeData>(DEFAULT_PATH), (UD) => { upgradeDict.Add(UD.ID, UD);});
        }

        //add upgrades with no prerequisites to possibleUpgrades, upgrades with prerequisite to futureUpgrades
        possibleUpgrades = new List<UpgradeData>();
        futureUpgrades = new List<UpgradeData>();
        unavailableUpgrades = new List<UpgradeData>();
        foreach (UpgradeData upgrade in allUpgrades)
        {
            //Debug.Log(upgrade.printDescription());
            if (upgrade.prerequisites == null || upgrade.prerequisites.Length==0)
            {
                possibleUpgrades.Add(upgrade);
            }
            else if(upgrade!=null)
            {
                futureUpgrades.Add(upgrade);
            }
            else
            {
                Debug.LogWarning($"UpgradeData {upgrade.ID} has a null value in prerequisites, remove it");
            }
        }
        
        Debug.Log(ToString());
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
    // If

    public UpgradeData pullUpgrade(string ID)
    {
        UpgradeData u = findUpgrade(ID);
        if (u != null && !u.isRepeatable) { 
            possibleUpgrades.Remove(u);
        }
        updateUpgrades();
        Debug.Log(ToString());
        return u;
    }

    //Pulls given upgrade and removes it from possibleUpgrades if it exists, else returns null
    public UpgradeData pullUpgrade(UpgradeData u)
    {
        return pullUpgrade(u.ID);
    }

    //Pulls random upgrade and removes it from possibleUpgrades
    public UpgradeData pullUpgrade()
    {
        int rand = UnityEngine.Random.Range(0, possibleUpgrades.Count);
        UpgradeData u = possibleUpgrades[rand];
        pullUpgrade(u);
        return u;
    }

    //Sees if any future upgrades are now possible to acquire, or are now unavailable
    private void updateUpgrades()
    {
        //Debug.Log($"Checking for available upgrades in futureUpgrade({futureUpgrades.Count})...");
        for(int i = futureUpgrades.Count-1; i >= 0; i--) 
        {
            UpgradeData u = futureUpgrades[i];
            //Debug.Log($"Looking at upgrade {u}");

            //NEEDS TO BE MOVED TO SEPERATE METHOD
            //removes from future if mutaully exclusive not in future or unavaialbe upgrades(meaning its been acquired)
            bool removed = false;
            foreach (UpgradeData e in u.mutuallyExclusives)
            {
                if (!possibleUpgrades.Contains(e) && !futureUpgrades.Contains(e) && !unavailableUpgrades.Contains(e)) //means upgrade e has been acquired
                {
                    removeMutallyExclusiveUpgrade(e);
                    removed = true;
                    //Debug.Log($"Upgrade {u} is nolonger available because its exlusive with upgrade {e}");
                    break;
                }
            }
            if (removed) continue;


            //adds to possibleUpgrades if no prerequisites in future or unavailable
            bool hasPreq = false;
            foreach (UpgradeData p in u.prerequisites)
            {
                if (possibleUpgrades.Contains(p) || futureUpgrades.Contains(p) || unavailableUpgrades.Contains(p)) //means upgrade has not been acquired
                {
                    hasPreq = true;
                    //Debug.Log($"Upgrade {u} still needs prereq {p}");
                    break;
                }
            }
            if (!hasPreq)
            {
                possibleUpgrades.Add(u);
                futureUpgrades.Remove(u);
                //Debug.Log($"Upgrade {u} has all prereqs, adding to possibleUpgrades");
            }
        }
    }

    //remove recursively upgrade u, all dependencies of upgrade u. and all dependencies of upgrade u's descedents
    //removing from both possibleFuture and futureUpgrades and added to unavailable upgrades
    UpgradeData removeMutallyExclusiveUpgrade(UpgradeData u)
    {
        foreach(UpgradeData p in u.prerequisites)
        {
            removeMutallyExclusiveUpgrade(p);
        }

        if (possibleUpgrades.Contains(u))
        {
            possibleUpgrades.Remove(u);
            unavailableUpgrades.Add(u);
        }
        else if (futureUpgrades.Contains(u)) 
        { 
            futureUpgrades.Remove(u); 
            futureUpgrades.Add(u);
        }
        else
        {
            Debug.LogError($"Upgrade {u} being removed does not exist in possible or future upgrades.");
        }
            return u;
    }


    //Sample upgrade space, recieve list of random possible upgrades without removing or applying them
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
        return $"possibleUpgrades: [{string.Join(", ", possibleUpgrades)}]\n" +
            $"futureUpgrades: [{string.Join(", ", futureUpgrades)}]\n" +
            $"unavailableUpgrades: [{string.Join(", ", unavailableUpgrades)}]";
    }

}
