using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    CharacterController controller;
    InputAction move;

    Vector3 movement;
    Vector3 input;

    float speed;
    public float runSpeed;

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
        }
        OnEnable();
    }

    void InputHandle()
    {
        input = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);
    }

    void GroundMovement()
    {
        speed = runSpeed;
        if (input.x != 0)
        {
            movement.x += input.x * speed;
        }
        else
        {
            movement.x = 0;
        }
        if (input.z != 0)
        {
            movement.z += input.z * speed;
        }
        else
        {
            movement.z = 0;
        }
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();
        GroundMovement();
        controller.Move(movement * Time.deltaTime);
    }
}
