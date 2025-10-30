using UnityEngine;
using System;

/*
 * Contains all the data for an upgrade, inherits upgradeable stat fields from UpgradeableStatContainer
 */
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData", order =1)]
public class UpgradeData : UpgradeableStatContainer
{
    public string ID;
    public bool isRepeatable;
    public UnlockFunctions.Unlockable[] unlocks = new UnlockFunctions.Unlockable[0];
    public string description;

    public override string ToString()
    {
        return ID;
    }
}
