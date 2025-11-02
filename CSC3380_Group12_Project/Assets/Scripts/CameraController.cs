using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public float sensitivity;
    public Transform orientation;
    public InputAction look;

    private float minX = -90f;
    private float maxX = 90f;

    private float rotY = 0f;
    private float rotX = 0f;


    
    private void OnEnable()
    {
        look.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensitivity = 60f;
        // Hides + Locks Cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inputs
        if(InputSystem.actions)
        {
            look = InputSystem.actions.FindAction("Player/Look");
            OnEnable();
        }

        //For portals to disable this script, through ControlScriptReference
        ControlScriptReference.ScriptsEnabled += Enable;
        ControlScriptReference.ScriptsDisabled += Disable;

    }
    private void Enable()
    {
        this.enabled = true;
    }

    private void Disable()
    {
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets mouse input
        rotY += look.ReadValue<Vector2>().x * Time.deltaTime * sensitivity;
        rotX += -look.ReadValue<Vector2>().y * Time.deltaTime * sensitivity;

        // Limits how far up and down the player can look
        rotX = Mathf.Clamp(rotX, minX, maxX);

        // Moves the camera using the inputs
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);

    }
    
    // Changes the sensitivity to NewMouseSensitivity
    public void SetMouseSensitivity(float NewMouseSensitivity)
    {
        sensitivity = NewMouseSensitivity;
        Debug.Log("Sensitivity set to: " + sensitivity);

    }
}
