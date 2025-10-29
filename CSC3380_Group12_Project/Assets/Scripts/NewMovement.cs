using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour
{

    //public Transform groundCheck;
    //public LayerMask groundMask;
    //
    //bool isGrounded;
    //bool isSprinting;
    //bool isCrouching;
    //bool isSliding;
    //
    //Vector3 movement;
    //Vector3 input;
    //
    //float speed;
    //public float runSpeed;
    //public float airSpeed;
    //public float sprintSpeed;
    //public float crouchSpeed;
    //public float slideSpeedIncrease;
    //public float slideSpeedDecrease;
    //Vector3 yVelocity;
    //Vector3 forwardDirection;
    //
    //float gravity;
    //public float normalGravity;
    //
    //float lastJumpTime = 0;
    //int jumpCharges;
    //public int maxJumpCharges;
    //public float jumpHeight;
    //
    ////these values might need to be adjusted later if we move the player
    //float startHeight;
    //float crouchHeight = 0.5f;
    //Vector3 crouchingCenter = new Vector3(0, 1.25f, 0);
    //Vector3 standingCenter = new Vector3(0, 1, 0);
    //
    //float slideTimer;
    //public float maxSlideTimer;
    //
    //float moveSpeed;

    Rigidbody body;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;
    public Transform orientation;
    public MovementState state;

    float vertInput;
    float horzInput;

    Vector3 moveDir;

    [Header("Jumping")]
    public float jumpPower;
    public int maxJumpCount;
    public float airMultiplier;
    float lastJumpTime;
    int jumpCount;
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeDetect;

    [Header("Ground Check")]
    public LayerMask groundMask;
    float playerHeight = 2;
    bool isGrounded;

    [Header("Inputs")]
    public InputAction move;
    public InputAction jump;
    public InputAction sprint;
    public InputAction crouch;



    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        sprint.Enable();
        crouch.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        sprint.Disable();
        crouch.Disable();
    }

    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        airborne
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        startYScale = transform.localScale.y;
        
        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
            sprint = InputSystem.actions.FindAction("Player/Sprint");
            crouch = InputSystem.actions.FindAction("Player/Crouch");
            OnEnable();
        }
    }

    private void Update()
    {
        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundMask);
        
        if (isGrounded && (Time.time - lastJumpTime > 0.25))
        {
            jumpCount = maxJumpCount;
            canJump = true;
        }

        InputHandle();
        SpeedControl();

        // Drag Handler
        if (isGrounded)
        {
            body.linearDamping = groundDrag;
        }
        else
        {
            body.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Handles all the movement inputs and changes the movement states
    private void InputHandle()
    {
        horzInput = move.ReadValue<Vector2>().x;
        vertInput = move.ReadValue<Vector2>().y;

        // Crouching
        if (isGrounded && crouch.IsPressed())
        {
            // Changes state and speed
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Sprinting
        else if (isGrounded && sprint.IsPressed())
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Airborne
        else
        {
            state = MovementState.airborne;
        }

        // Jumping
        if (jump.WasPressedThisFrame() && canJump && jumpCount > 0)
        {
            Jump();

            if (jumpCount == 0)
            {
                canJump = false;
            }
        }
        else if (jump.IsPressed() && isGrounded && Time.time - lastJumpTime > 0.5)
        {
            Jump(); 
            // Allows the player to be able to hold the jump key and auto jump when they hit the ground again
        }

        if (crouch.WasPressedThisFrame())
        {
            // Shrinks the player and pushes them to the floor
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            body.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (crouch.WasReleasedThisFrame())
        {
            // Enlarges the player back to normal size
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        
    }

    private void MovePlayer()
    {
        // Move direction
        moveDir = orientation.forward * vertInput + orientation.right * horzInput;

        // on a slope
        if (OnSlope())
        {
            body.AddForce(20f * moveSpeed * GetSlopeMoveDirection(), ForceMode.Force);
            
            body.AddForce(Vector3.down * 100f, ForceMode.Force);
        }
        
        // on ground
        if (isGrounded)
        {
            body.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
        }
        // when airborne
        else
        {
            body.AddForce(10f * airMultiplier * moveSpeed * moveDir.normalized, ForceMode.Force);
        }

        body.useGravity = !OnSlope();
        
    }

    private void SpeedControl()
    {
        // Limits the speed on slope
        if (OnSlope())
        {
            if (body.linearVelocity.magnitude > moveSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * moveSpeed;
            }
        }

        // Limits the speed on ground and airborne
        else
        {
            Vector3 flatVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);

            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocty = flatVelocity.normalized * moveSpeed;
                body.linearVelocity = new Vector3(limitedVelocty.x, body.linearVelocity.y, limitedVelocty.z);
            }
        }
        
    }

    private void Jump()
    {
        body.linearVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
        body.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        lastJumpTime = Time.time;
        jumpCount--;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeDetect, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeDetect.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeDetect.normal).normalized;
    }

}
