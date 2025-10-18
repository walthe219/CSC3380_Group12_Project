using UnityEngine;
using System;

/*
 * Contains all the data for an upgrade, inherits stat fields from stats containers
 */
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData", order =1)]
public class UpgradeData : StatContainer
{
    public string ID;
    public UnlockFunctions.Unlockable[] unlocks = new UnlockFunctions.Unlockable[0];

    /*
     * Invokes unlock events from UnlockFunctions for each unlock attached to this upgrade
     */
    public void activate()
    {
        foreach (UnlockFunctions.Unlockable unlock in unlocks)
        {
            Action unlockCall = UnlockFunctions.getAction(unlock);
            if (unlockCall == null)
            {
                Debug.LogWarning($"This Event can not be invoked on {ID} because it has no subscribers");
                continue;
            }
            unlockCall();
        }
    }
}
