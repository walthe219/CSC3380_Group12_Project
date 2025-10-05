using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    CharacterController controller;
    public InputAction move;
    public InputAction jump;
    public InputAction sprint;

    public Transform groundCheck;
    public LayerMask groundMask;

    bool isGrounded;
    bool isSprinting;

    Vector3 movement;
    Vector3 input;

    float speed;
    public float runSpeed;
    public float airSpeed;
    public float sprintSpeed;
    Vector3 yVelocity;

    float gravity;
    public float normalGravity;

    float lastJumpTime;
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
            sprint = InputSystem.actions.FindAction("Player/Sprint");
            OnEnable();
        }
        lastJumpTime = 0;
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
        CheckGround();
        controller.Move(movement * Time.deltaTime);
        ApplyGravity();
    }

    // Handles all the movement inputs
    void InputHandle()
    {
        input = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (jump.WasPressedThisFrame() && jumpCharges > 0)
        {
            Jump();   
        }
        if(sprint.IsPressed() && isGrounded)
        {
            isSprinting = true;
        }
        if(!sprint.IsPressed())
        {
            isSprinting = false;
        }
    }

    // Movement settings when on the ground
    void GroundMovement()
    {
        // if sprinting use sprintSpeed, if not sprinting use runSpeed
        speed = isSprinting ? sprintSpeed : runSpeed;
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

    // Movement settings when in the air
    void AirMovement()
    {
        movement.x += input.x * airSpeed;
        movement.z += input.z * airSpeed;
         
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    // Checks if the player is on the ground, resets jump charges if TRUE
    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);

        if(isGrounded && (Time.time - lastJumpTime > 0.1))
        {
            jumpCharges = maxJumpCharges;
        }
    }

    // Jump
    void Jump()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
        jumpCharges--;
        lastJumpTime = Time.time;
    }

    // Applies gravity to the player
    void ApplyGravity()
    {
        gravity = normalGravity;
        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }
}
