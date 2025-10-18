using System.Collections;
using UnityEngine;

/*
 * Defines an Upgrade object, takes in UpgradeData
 */
public class Upgrade
{
    public UpgradeData data;

    public Upgrade(UpgradeData d)
    {
        data = d;
    }

    public void activate()
    {
        data.activate();
    }
}