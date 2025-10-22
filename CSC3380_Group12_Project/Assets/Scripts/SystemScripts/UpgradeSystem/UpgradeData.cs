using UnityEngine;
using System;

/*
 * Contains all the data for an upgrade, inherits upgradeable stat fields from StatsContainer
 */
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData", order =1)]
public class UpgradeData : StatContainer
{
    public string ID;
    public UnlockFunctions.Unlockable[] unlocks = new UnlockFunctions.Unlockable[0];
    public string description;
}
