using System;
using UnityEngine;

/*
 * Defines an Upgrade object, takes in UpgradeData that defines which upgrade it is, can be activated to call unlock functions defined in data
 */
public class Upgrade
{
    public UpgradeData data;
    public Upgrade(UpgradeData d)
    {
        data = d;
    }

    /*
     * Invokes unlock events from UnlockFunctions for each unlock attached to this upgrade
     */
    public void activate()
    {
        foreach (UnlockFunctions.Unlockable unlock in data.unlocks)
        {
            UnlockFunctions.callAction(unlock);
        }

    }
}