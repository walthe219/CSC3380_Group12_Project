using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    CharacterController controller;
    public InputAction move;
    public InputAction jump;

    public Transform groundCheck;
    public LayerMask groundMask;

    bool isGrounded;

    Vector3 movement;
    Vector3 input;

    float speed;
    public float runSpeed;
    public float airSpeed;
    Vector3 yVelocity;

    float gravity;
    public float normalGravity;

    int jumpCharges;
    public int maxJumpCharges;
    public float jumpHeight;

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
            OnEnable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();
        if(isGrounded)
        {
            GroundMovement();
        }
        else
        {
            AirMovement();
        }
        checkGround();
        controller.Move(movement * Time.deltaTime);
        ApplyGravity();
    }

    void InputHandle()
    {
        input = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (jump.IsPressed() && jumpCharges > 0)
        {
            Jump();
        }
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

    void AirMovement()
    {
        movement.x += input.x * airSpeed;
        movement.z += input.z * airSpeed;

        movement = Vector3.ClampMagnitude(movement, speed);
    }

    void checkGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if(isGrounded)
        {
            jumpCharges = maxJumpCharges;
        }
    }

    void Jump()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
        jumpCharges--;
    }

    void ApplyGravity()
    {
        gravity = normalGravity;
        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }
}
