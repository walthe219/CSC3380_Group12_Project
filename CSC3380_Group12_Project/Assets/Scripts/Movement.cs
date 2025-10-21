using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    public InputAction move;
    public InputAction jump;
    public InputAction sprint;
    public InputAction crouch;

    public Transform groundCheck;
    public LayerMask groundMask;

    bool isGrounded;
    bool isSprinting;
    bool isCrouching;
    bool isSliding;

    Vector3 movement;
    Vector3 input;
    Vector3 moveDirection;

    float speed;
    public float runSpeed;
    public float airSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float slideSpeedIncrease;
    public float slideSpeedDecrease;
    Vector3 yVelocity;
    Vector3 forwardDirection;

    float gravity;
    public float normalGravity;

    float lastJumpTime = 0;
    int jumpCharges;
    public int maxJumpCharges;
    public float jumpHeight;

    //these values might need to be adjusted later if we move the player
    float startHeight;
    float crouchHeight = 0.5f;
    Vector3 crouchingCenter = new Vector3(0, 1.25f, 0); 
    Vector3 standingCenter = new Vector3(0, 1, 0);

    float slideTimer;
    public float maxSlideTimer;

    public float maxSlopeAngle;
    float slopeAngle;
    RaycastHit slopeTouch;

    Transform playerPos;
    bool isFalling; // Not actually falling, only when then player's y-position is decreasing
    float prevYPos;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerPos = GetComponent<Transform>();
        startHeight = transform.localScale.y;
        prevYPos = playerPos.position.y;
        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
            sprint = InputSystem.actions.FindAction("Player/Sprint");
            crouch = InputSystem.actions.FindAction("Player/Crouch");
            OnEnable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        HeightChange();
        InputHandle();
        if(isGrounded && !isSliding)
        {
            GroundMovement();
        }
        else if (!isGrounded)
        {
            AirMovement();
        }
        else if (isSliding)
        {
            SlideMovement();
            DecreaseSpeed(slideSpeedDecrease);
            slideTimer -= 1f * Time.deltaTime;
            if (slideTimer < 0)
            {
                isSliding = false;
            }
        }
        
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
        if(sprint.WasPressedThisFrame() && isGrounded)
        {
            isSprinting = true;
        }
        if(sprint.WasReleasedThisFrame())
        {
            isSprinting = false;
        }
        if(crouch.WasPressedThisFrame())
        {
            Crouch();
        }
        if(crouch.WasReleasedThisFrame())
        {
            Uncrouch();
        }
    }

    // Movement settings when on the ground
    void GroundMovement()
    {
        speed = isSprinting ? sprintSpeed : isCrouching ? crouchSpeed: runSpeed;
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

    // Movement settings when sliding, currently can only go forwards when sliding unless cancelled
    void SlideMovement()
    {
        movement += forwardDirection;
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    // Checks if the player is on the ground, resets jump charges if TRUE
    void CheckGround()
    {
        bool currState = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if(!currState && isGrounded)
        {
            forwardDirection = transform.forward;
        }

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

    void Crouch()
    {
        controller.height = crouchHeight;
        controller.center = crouchingCenter;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        isCrouching = true;
        if (speed > runSpeed)
        {
            forwardDirection = transform.forward;
            isSliding = true;
            if (isGrounded && isFalling)
            {
                IncreaseSpeed(slideSpeedIncrease*3);
            }
            else if (isGrounded)
            {
                IncreaseSpeed(slideSpeedIncrease);
            }
            slideTimer = maxSlideTimer;
        }
    }

    void Uncrouch()
    {
        controller.height = startHeight * 2;
        controller.center = standingCenter;
        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);
        isCrouching = false;
        isSliding = false;
    }

    void IncreaseSpeed(float speedIncrease)
    {
        speed += speedIncrease;
    }

    void DecreaseSpeed(float speedDecrease)
    {
        speed -= speedDecrease * Time.deltaTime;
    }

    void ApplyGravity()
    {
        gravity = normalGravity;
        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeTouch, crouchHeight * 0.5f + 0.3f))
        {
            slopeAngle = Vector3.Angle(Vector3.up, slopeTouch.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeTouch.normal).normalized;
    }

    void HeightChange()
    {
        float currYPos = playerPos.position.y;
        isFalling = currYPos < prevYPos ? true : false;
        prevYPos = currYPos;
    }
}
