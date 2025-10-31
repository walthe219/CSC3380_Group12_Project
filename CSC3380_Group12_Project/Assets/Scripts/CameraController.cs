using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private float minX = -90f;
    private float maxX = 90f;

    public float sensitivity;
    public Transform orientation;
    public InputAction look;
    
    float rotY = 0f;
    float rotX = 0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Gets mouse input
        rotY += look.ReadValue<Vector2>().x * Time.deltaTime * sensitivity;
        rotX += -look.ReadValue<Vector2>().y * Time.deltaTime * sensitivity;

        rotX = Mathf.Clamp(rotX, minX, maxX);

        //transform.localEulerAngles = new Vector3(0, rotY, 0);
        //cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);
    }
    public void SetMouseSensitivity(float NewMouseSensitivity)
    {
        sensitivity = NewMouseSensitivity;
        Debug.Log("Sensitivity set to: " + sensitivity);

    }
}
