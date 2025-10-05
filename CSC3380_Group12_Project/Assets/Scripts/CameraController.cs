using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public float minX = -60f;
    public float maxX = 60f;

    public float sensitivity;
    public Camera cam;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(InputSystem.actions)
        {
            look = InputSystem.actions.FindAction("Player/Look");
        }
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        
        rotY += look.ReadValue<Vector2>().x * sensitivity;
        rotX += look.ReadValue<Vector2>().y * sensitivity;

        rotX = Mathf.Clamp(rotX, minX, maxX);

        transform.localEulerAngles = new Vector3(0, rotY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
    }
}
