using UnityEngine;
using System;

/*
 * Contains all the data for an upgrade, inherits upgradeable stat fields from UpgradeableStatContainer
 * To create new data for an upgrade:
 *  Go to Assets/Resources/UpgradeData, then in the folder click Create/ScriptableObject/UpgradeData
 *  Then define that stats for that object
 */
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData", order =1)]
public class UpgradeData : UpgradeableStatContainer
{
    [Tooltip("Name used to represent the upgade in game and Console")]
    public string ID; 
    [Tooltip("If repeateable, then upgrade can be earned infinite times")]
    public bool isRepeatable; 
    [Tooltip("List of unlocks acquired from this upgrade")]
    public UnlockFunctions.Unlockable[] unlocks = new UnlockFunctions.Unlockable[0];
    [Tooltip("Description of upgrades affects")]
    public string description;

    public override string ToString()
    {
        return ID;
    }
}
