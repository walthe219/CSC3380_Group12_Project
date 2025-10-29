using UnityEngine;
using UnityEngine.InputSystem;


public class HoldTabMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    private InputAction holdTab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Create a new InputAction for Tab as a button
        holdTab = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");

        // Show menu while Tab is held
        holdTab.performed += _ => menuPanel.SetActive(true);

        // Hide menu when Tab is released
        holdTab.canceled += _ => menuPanel.SetActive(false);
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
}
