using UnityEngine;
using UnityEngine.InputSystem;


public class cameraScript : MonoBehaviour
{
    public float mouseSens = 20f;
    public float yLimit = 89f;
    public GameObject cam;

    Vector2 rotation = Vector2.zero;

    InputAction look;

    private void Start()
    {
        look = InputSystem.actions.FindAction("Look");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rotation.x += look.ReadValue<Vector2>().x *mouseSens * Time.deltaTime;
        rotation.y += look.ReadValue<Vector2>().y * mouseSens * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -yLimit, yLimit);

        transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
        cam.transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
    }



    /*
    public float mouseSensitivity = 20f;
    public float yCamLimit = 90f;
    public GameObject cam;


    InputAction look;
    Vector2 rotation = Vector2.zero;

    private void Start()
    {
        look = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {

        rotation.x += look.ReadValue<Vector2>().x * mouseSensitivity;
        rotation.y += Mathf.Clamp(look.ReadValue<Vector2>().y, -yCamLimit, yCamLimit) * mouseSensitivity;
    }

    private void FixedUpdate()
    {
        cam.transform.localRotation = Quaternion.AngleAxis(rotation.x,Vector3.up);
        transform.localRotation = Quaternion.AngleAxis(rotation.y,Vector3.left);
    }
    */
}
