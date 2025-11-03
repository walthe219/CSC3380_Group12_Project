using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class HoldTabMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] TextMeshProUGUI upgradesText;
    [SerializeField] TMP_Text statsText;
    [SerializeField] PlayerStats currentStats;
    

    private InputAction holdTab;

    private void Awake()
    {
        // Setup Tab input
        holdTab = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");
        holdTab.performed += _ => menuPanel.SetActive(true);
        holdTab.canceled += _ => menuPanel.SetActive(false);
    }

    void Start()
    {
        /*
        // Create dummy upgrades for testing
        UpgradeData dashData = ScriptableObject.CreateInstance<UpgradeData>();
        dashData.ID = "Dash";

        UpgradeData jumpData = ScriptableObject.CreateInstance<UpgradeData>();
        jumpData.ID = "ExtraJump";

        // Directly add them to acquiredUpgrades
        // NOTE: This works only if acquiredUpgrades is public or has a public method to expose it
        upgradeManager.GetAcquiredUpgrades().Add(new Upgrade(dashData));
        upgradeManager.GetAcquiredUpgrades().Add(new Upgrade(jumpData));

        // Update TMP
        UpdateUpgradeText();
        */
    }

    private void OnEnable()
    {
        holdTab.Enable();

        if (menuPanel != null)
            menuPanel.SetActive(false);

        //Subscribe to dash event
        //UnlockFunctions.UnlockDashEvent += OnDashUnlocked;
    }

    private void OnDisable()
    {
        holdTab.Disable();

        //unsubscribe to the dash event
        //UnlockFunctions.UnlockDashEvent -= OnDashUnlocked;
    }

   /* private void OnDashUnlocked()
{
    // Create the Upgrade from the assigned Dash UpgradeData SO
    UpgradeData dashUnlocked = ScriptableObject.CreateInstance<UpgradeData>();
    dashUnlocked.ID = "Dash";
    upgradeManager.GetAcquiredUpgrades().Add(new Upgrade(dashUnlocked));

    // Update TMP display
    UpdateUpgradeText();

    //Debug.Log("Dash upgrade applied via event!");
}*/

    private void UpdateUpgradeText()
    {
        //if (upgradesText == null) return;

        List<Upgrade> acquired = UpgradeManager.Instance.acquiredUpgrades;

        upgradesText.text = string.Join(",", acquired);
       /* upgradesText.text = "";

        for (int i = 0; i < acquired.Count; i++)
        {
            upgradesText.text += acquired[i].data.ID;
            if (i < acquired.Count - 1)
                upgradesText.text += ", ";
        }*/

        
         //if(acquired.Count!=0)Debug.Log("Acquired upgrades: " + upgradesText.text + ": " + acquired.Count);
        
       
    }

   

    private void Update()
    {   

        //if (menuPanel.activeSelf && upgradesText != null)
        //{
        UpdateUpgradeText();
        //}

        string s = "";

        string[] stats = {"Health", "Stamina", "Ammo", "Damage", "MoveSpeed", "NumJumps", "SlidePower", "DashPower" }; 
        float[] values = {currentStats.health, currentStats.stamina, currentStats.ammo, currentStats.damage, currentStats.moveSpeed, currentStats.numJumps, currentStats.slidePower, currentStats.dashPower};
        for(int i = 0; i < 8; i++)
        {
            s += stats[i] + ": " + values[i] + "\n";
        }
        statsText.text = s;
    }
}
