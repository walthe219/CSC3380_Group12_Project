using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class HoldTabMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] UpgradeManager upgradeManager;
    private InputAction holdTab;
    public TextMeshProUGUI upgradesText;
    


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Create a new InputAction for Tab as a button
        holdTab = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");

        // Show menu while Tab is held
        holdTab.performed += _ => menuPanel.SetActive(true);

        // Hide menu when Tab is released
        holdTab.canceled += _ => menuPanel.SetActive(false);

        upgradeManager.OnUpgradeAdded += UpdateUpgradeText;
    }

    private void Start()
    {

        

    // Quick test add a dummy upgrade if none exist

    }

    private void OnEnable()
    {
        holdTab.Enable();

        // Ensure menu starts hidden
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    private void OnDisable()
    {
        holdTab.Disable();
    }

    public void UpdateUpgradeText()
{
    if (upgradesText != null)
    {
        Debug.Log("Acquired upgrades: " + string.Join(", ", upgradeManager.GetAcquiredUpgrades().Select(u => u.data.ID)));
        upgradesText.text = string.Join(", ", upgradeManager.GetAcquiredUpgrades().Select(u => u.data.ID));
    }
}

    private void Update()
    {
        if (menuPanel.activeSelf && upgradesText != null)
        {
            // Convert the list of upgrades to a comma-separated string
            string upgradesList = string.Join(", ", upgradeManager.GetAcquiredUpgrades().Select(u => u.data.ID));
            upgradesText.text = upgradesList;
        }
    }
}
