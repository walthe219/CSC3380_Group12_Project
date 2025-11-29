using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.TestTools;


//Unit Tests for UpgradeSpace, can run in Edit Mode
public class UpgradeSpaceTester
{
    static UpgradeSpace upgradeSpace;
    static UpgradeData[] testUpgrades; 
    static Dictionary<string, UpgradeData> testUpgradesDict;
    UpgradeData[] upgradesWithPreqs;

    //Testing Variables need to be adjusted based on upgrades in UpgradeSpaceTest folder
    const string TEST_FOLDER_PATH = "TestUpgrades";
    const int NUMBER_OF_TEST_UPGRADES = 6;
    string[] preqIDS = {"Test1","Test3"};

    [SetUp]
    public void Setup()
    {
        testUpgrades = Resources.LoadAll<UpgradeData>(TEST_FOLDER_PATH);
        testUpgradesDict = new Dictionary<string, UpgradeData>();
        Array.ForEach(testUpgrades, (u) =>testUpgradesDict.Add(u.ID,u));
        upgradeSpace = new UpgradeSpace(null, TEST_FOLDER_PATH);
        Debug.Log(upgradeSpace.ToString());
        upgradesWithPreqs = new UpgradeData[preqIDS.Length];
        for(int i = 0; i < preqIDS.Length; i++)
        {
            upgradesWithPreqs[i] = testUpgradesDict[preqIDS[i]];
        }
    }


    [Test]
    public void TestClassNumTestUpgrades_LoadingTestUpgrades_NumUpgradesInTestFolder()
    {
        Assert.AreEqual(NUMBER_OF_TEST_UPGRADES,testUpgrades.Length, "The test class does not have the correct number of test upgrades set, " +
            "if you have changed the number of test upgrades, the test cases need to be updated to include the new upgrades");
    }

    [Test]
    public void TestClassUpgradesWithPreqs_LoadingUpgradesWithPrereqs_ListOfAllPreqUpgrades()
    {
        UpgradeData[] result = new UpgradeData[upgradesWithPreqs.Length];
        int j = 0;
        for(int i = 0; i < testUpgrades.Length; i++)
        {
            UpgradeData u = testUpgrades[i];
            if (u.prerequisites != null && u.prerequisites.Length > 0) 
            {
                result[j++] = u;
            }
        }
        CollectionAssert.AreEqual(upgradesWithPreqs,result);
    }

    //Test findUpgrade(UpgradeData)
    [Test]
    public void findUpgrade_InvalidID_ThrowCantFindError()
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
    public void findUpgrade_AfterPull_ThrowCantFindError()
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
    public void pullUpgrade_PullSameTwice_ThrowCantFindError()
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
    [Test]
    public void upgradeDict_MultipleUpgradeSpaces_UpgradeDataDict()
    {
        UpgradeSpace other = new UpgradeSpace();
        Dictionary<String,UpgradeData> expected = new Dictionary<String,UpgradeData>();
        UpgradeData[] realUgprades = Resources.LoadAll<UpgradeData>("UpgradeData");
        Array.ForEach(realUgprades, (u)=>expected.Add(u.ID,u));
        CollectionAssert.AreEqual(expected, UpgradeSpace.upgradeDict);
    }

    //Test Printouts
    [Test]
    public void UpgradeDataprintDescription_StatUpgrade_OnlyStats()
    {
        var upgrade = upgradeSpace.pullUpgrade("Test0");
        var result = upgrade.printDescription(label:false);
        Assert.AreEqual("Health: 10\nStamina: 10\nAmmo: 2\r\n", result);
    }
    [Test]
    public void UpgradeDataprintDescription_UnlockUpgrade_OnlyUnlocks()
    {
        upgradeSpace.pullUpgrade("Test0");
        var upgrade = upgradeSpace.pullUpgrade("Test1");
        var result = upgrade.printDescription(label: false);
        Assert.AreEqual("Unlocks: DASH, SLIDE\r\n", result);
    }

    [Test]
    public void UpgradeDataprintDescription_printLabelNonRepeatable_LabelRarityPrinted()
    {
        var upgrade = upgradeSpace.pullUpgrade("Test0");
        var result = upgrade.printDescription(ID:true,label:true,stats:false,unlock:false,descr:false);
        Assert.AreEqual("Test0[COMMON]\r\n", result);
    }

    [Test]
    public void UpgradeDataprintDescription_printLabelRepeatable_LabelRarityRepeatablePrinted()
    {
        var upgrade = upgradeSpace.pullUpgrade("TestRepeatable");
        var result = upgrade.printDescription(ID:true,label: true, stats: false, unlock: false, descr: false);
        Assert.AreEqual("TestRepeatable[COMMON][REPEATABLE]\r\n", result);
    }

    [Test]
    public void UpgradeDataprintDescription_AllFieldsUpgrade_FullDescription()
    {
        var upgrade = upgradeSpace.pullUpgrade("Test2");
        var result = upgrade.printDescription(label: false);
        Assert.AreEqual("Health: 10\nStamina: 10\nAmmo: 2\r\nUnlocks: DASH, SLIDE\r\nDescription:\r\nThis is a test", result);
    }

    /*
     * Test Dependencies
     */
    [Test]
    public void newUpgradeSpace_UpgradesWithPreqs_InFutureUpgrades()
    {
        foreach (UpgradeData u in upgradesWithPreqs)
        {
            Assert.True(upgradeSpace.ROfutureUpgrades.Contains(u));
            Assert.False(upgradeSpace.ROpossibleUpgrades.Contains(u));
        }
    }

    [Test]
    public void newUpgradeSpace_UpgradesWithouthPreqs_InPossibleUpgrades()
    {
        foreach (UpgradeData u in testUpgrades)
        {
            if(!upgradesWithPreqs.Contains(u))
            {
                Assert.False(upgradeSpace.ROfutureUpgrades.Contains(u));
                Assert.True(upgradeSpace.ROpossibleUpgrades.Contains(u));
            }
        }
    }

    [Test] 
    public void pullUpgrade_AfterUpgradePreqAcquired_ReturnUpgrade()
    {
        upgradeSpace.pullUpgrade("Test0");
        var result = upgradeSpace.pullUpgrade("Test1");
        Assert.AreEqual(testUpgradesDict["Test1"], result);
    }

    [Test]
    public void pullUpgrade_PrereqNotAcquiredYet_ThrowCantFindError()
    {
        var result = upgradeSpace.pullUpgrade("Test1");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID Test1 in possibleUpgrades");
    }

    [Test]
    public void pullUpgrade_AfterMultiplePreqsAcquired_ReturnUpgrade()
    {
        upgradeSpace.pullUpgrade("Test0");
        upgradeSpace.pullUpgrade("Test1");
        upgradeSpace.pullUpgrade("Test2");
        var result = upgradeSpace.pullUpgrade("Test3");
        Assert.AreEqual(testUpgradesDict["Test3"], result);
    }

    public void pullUpgrade_OnlyOnePreqAcquired_ThrowCantFindError()
    {
        upgradeSpace.pullUpgrade("Test0");
        var result = upgradeSpace.pullUpgrade("Test3");
        LogAssert.Expect(LogType.Error, "Could not find upgrade with ID Test3 in possibleUpgrades");
    }


    //Test Mutually Exclusives
}

