using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;


//Unit Tests for UpgradeSpace, can run in Edit Mode
public class UpgradeSpaceTester
{
    static UpgradeSpace upgradeSpace;
    static UpgradeData[] testUpgrades; 
    static Dictionary<string, UpgradeData> testUpgradesDict; 

    [SetUp]
    public void Setup()
    {
        testUpgrades = Resources.LoadAll<UpgradeData>("UpgradeSpaceTest");
        testUpgradesDict = new Dictionary<string, UpgradeData>();
        Array.ForEach(testUpgrades, (u) =>testUpgradesDict.Add(u.ID,u));
        upgradeSpace = new UpgradeSpace(null, "UpgradeSpaceTest");
    }

    //Test findUpgrade(UpgradeData)
    [Test]
    public void findUpgrade_InvalidID_ReturnsError()
    {
        var result = upgradeSpace.findUpgrade("NonexistantUpgradeID");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID NonexistantUpgradeID in possibleUpgrades");
    }

    [Test]
    public void findUpgrade_ValidID_ReturnsUpgrade()
    {
        var result = upgradeSpace.findUpgrade("Test0");
        Assert.AreEqual(testUpgradesDict["Test0"],result);
    }

    [Test]
    public void findUpgrade_AfterPull_ReturnError()
    {
        var result = upgradeSpace.pullUpgrade("Test0");
        Assert.AreEqual(testUpgradesDict["Test0"], result);

        result = upgradeSpace.findUpgrade("Test0");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID Test0 in possibleUpgrades");
    }

    //Test pullUpgrade(UpgradeData)
    [Test]
    public void pullUpgrade_InvalidID_ReturnsUpgrade()
    {
        var result = upgradeSpace.pullUpgrade("NonexistantUpgradeID");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID NonexistantUpgradeID in possibleUpgrades");
    }
    [Test]
    public void pullUpgrade_ValidID_ReturnsUpgrade()
    {
        var result = upgradeSpace.pullUpgrade("Test0");
        Assert.AreEqual(testUpgradesDict["Test0"], result);
    }

    [Test]
    public void pullUpgrade_PullSameTwice_ReturnsError()
    {
        var result = upgradeSpace.pullUpgrade("Test0");
        Assert.AreEqual(testUpgradesDict["Test0"], result);

        result = upgradeSpace.pullUpgrade("Test0");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID Test0 in possibleUpgrades");
    }

    [Test]
    public void pullUpgrade_IsRepeatablePullTwice_ReturnUpgrade()
    {
        var result = upgradeSpace.pullUpgrade("TestRepeatable");
        Assert.AreEqual(testUpgradesDict["TestRepeatable"], result);

        result = upgradeSpace.findUpgrade("TestRepeatable");
        Assert.AreEqual(testUpgradesDict["TestRepeatable"], result);

        result = upgradeSpace.pullUpgrade("TestRepeatable");
        Assert.AreEqual(testUpgradesDict["TestRepeatable"], result);
    }

    //Test upgradeDict with multiple upgradespaces

    //Test Printouts

    //Test Dependencies

    //Test Mutually Exclusives
}
