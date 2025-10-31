using UnityEngine;
using UnityEngine.InputSystem;

public class MouseView : MonoBehaviour
{

    public float sensitivity = 100f;

    public Transform body;
    //public Transform cam;

    public InputAction lookMove;

    private float xRot = 0f;

    private void OnEnable()
    {
        lookMove = InputSystem.actions.FindAction("Look");
    }
    private void OnDisable()
    {
        lookMove.Disable();
    }

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        Vector2 looking = lookMove.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        /*float xMousePos = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float yMousePos = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;*/

        xRot -= looking.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * looking.x);

        /*
        float rotFactor = cam.rotation.x * (180 / Mathf.PI) * 2.0170612836f;

        if (rotFactor > -78f && rotFactor < 83f)
        {
            //Debug.Log(cam.rotation.x * (180/Mathf.PI) * 2.0170612836f);
            cam.Rotate(Vector3.right * -yMousePos);
        }*/

        //body.Rotate(Vector3.right * yMousePos);

    }

}