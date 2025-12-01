using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class NewMovement : MonoBehaviour
{

    [Header("Movement")]
    private Rigidbody body;
    public int actualSpeed;
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float dashSpeed;

    private float desiredSpeed;
    private float prevDesiredSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;
    public Transform orientation;
    public MovementState state;

    public bool isSliding;

    private Vector3 moveDir;

    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats basePlayerStats;

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
    private bool leavingSlope;

    [Header("Dashing")]
    public float dashForce;
    private float dashTime;
    public float maxDashTime;
    public bool isDashing = false;
    public float dashDrag;
    private bool dashUnlocked;
    private bool omniDashUnlocked = false;

    [Header("Stamina")]
    public float curStamina;
    private float maxStamina = 100f;
    private float staminaRechargeDelay = 2f;
    private float staminaRechargeTimer;
    private float staminaRechargeRate = 20f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    private float playerHeight = 2;
    public bool isGrounded;

    [Header("Inputs")]
    public InputAction move;
    public InputAction jump;
    public InputAction sprint;
    public InputAction crouch;
    public InputAction dash;

    private float vertInput;
    private float horzInput;

    [Header("Testing")]
    public float test;
    public Camera cam;
    public float fovChangeTime = 1;

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        sprint.Enable();
        crouch.Enable();
        if(dashUnlocked)
        {
            dash.Enable();
        }   
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        sprint.Disable();
        crouch.Disable();
        dash.Disable();
    }

    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        sliding,
        dashing,
        airborne
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        startYScale = transform.localScale.y;
        curStamina = maxStamina;
        UnlockFunctions.UnlockDashEvent += UnlockDash;
        UnlockFunctions.UnlockOmniDashEvent += UnlockOmniDash;

        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
            sprint = InputSystem.actions.FindAction("Player/Sprint");
            crouch = InputSystem.actions.FindAction("Player/Crouch");
            dash = InputSystem.actions.FindAction("Player/Dash");
            OnEnable();
            //dash.Disable();
        }

        //For portals to disable this script, through ControlScriptReference
        ControlScriptReference.ScriptsEnabled += Enable;
        ControlScriptReference.ScriptsDisabled += Disable;

        UpdateMovementValues();
    }

    private void Enable()
    {
        this.enabled = true;
    }

    private void Disable()
    {
        this.enabled = false;
    }

    // Updates all of the max movement values in the this script to the max values in basePlayerStats
    private void UpdateMovementValues()
    {
        walkSpeed = basePlayerStats.moveSpeed;
        sprintSpeed = walkSpeed + 3;
        crouchSpeed = walkSpeed - 2;

        maxStamina = basePlayerStats.stamina;
        staminaRechargeRate = maxStamina / 5;

        maxJumpCount = basePlayerStats.numJumps;

        dashForce = basePlayerStats.dashPower;
    }

    private void Update()
    {
        actualSpeed = (int)body.linearVelocity.magnitude;
        // Ground Check
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z), 0.4f, groundMask);

        // If the player is grounded for longer that 0.25 seconds it resets the jump count
        if (isGrounded && (Time.time - lastJumpTime > 0.25))
        {
            jumpCount = maxJumpCount;
            canJump = true;
            leavingSlope = false;
        }

        InputHandle();
        SpeedControl();

        // Updates movement values if an upgrade has been obtained
        if (walkSpeed != basePlayerStats.moveSpeed ||
            maxJumpCount != basePlayerStats.numJumps ||
            maxStamina != basePlayerStats.stamina ||
            dashForce != basePlayerStats.dashPower)
        {
            UpdateMovementValues();
        }

        // Start the recharge timer if stamina is below the max
        if(isGrounded && curStamina != maxStamina)
        {
            staminaRechargeTimer += Time.deltaTime;
        }

        // Start recharging stamina if stamina is below the max and the recharge timer has passed the delay
        if(curStamina != maxStamina && staminaRechargeTimer >= staminaRechargeDelay)
        {
            StaminaRecharge();
        }

        currPlayerStats.stamina = (int)curStamina;

        // Drag Handler
        if (isGrounded)
        {
            body.linearDamping = groundDrag;
        }
        else if (isDashing)
        {
            body.linearDamping = dashDrag;
        }
        else
        {
            body.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            DashTimer();
        }
        MovePlayer();
    }

    // Handles all the movement inputs and changes the movement states
    private void InputHandle()
    {
        horzInput = move.ReadValue<Vector2>().x;
        vertInput = move.ReadValue<Vector2>().y;

        // Sliding
        if (isSliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && body.linearVelocity.y < 0.1f)
            {
                desiredSpeed = slideSpeed;
            }
            else
            {
                desiredSpeed = sprintSpeed;
            }
        }

        // Crouching
        else if (isGrounded && crouch.IsPressed())
        {
            // Changes state and speed
            state = MovementState.crouching;
            desiredSpeed = crouchSpeed;
        }

        // Dashing
        else if (dash.IsPressed())
        {
            state = MovementState.dashing;
            //desiredSpeed = dashSpeed;
        }

        // Sprinting
        else if (isGrounded && sprint.IsPressed())
        {
            state = MovementState.sprinting;
            desiredSpeed = sprintSpeed;
        }

        // Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredSpeed = walkSpeed;
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
        // Allows the player to be able to hold the jump key and auto jump when they hit the ground again
        else if (jump.IsPressed() && isGrounded && Time.time - lastJumpTime > 0.5)
        {
            Jump();
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

        // Can only dash if the stamina is at least 50
        if (dash.WasPressedThisFrame() && curStamina >= 50 && ((horzInput != 0 || vertInput != 0) || omniDashUnlocked))
        {
            Dash();
        }

        // Checks for drastic change in desiredSpeed
        if (Mathf.Abs(desiredSpeed - prevDesiredSpeed) > 4f && moveSpeed != 0)
        {
            StopCoroutine(SmoothlyLerpMoveSpeed());
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredSpeed;
        }

        prevDesiredSpeed = desiredSpeed;
    }

    // Changes the move speed to the desired speed gradually over time instead of instantly changing it
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float diff = Mathf.Abs(desiredSpeed - moveSpeed);
        float start = moveSpeed;

        while (time < diff)
        {
            moveSpeed = Mathf.Lerp(start, desiredSpeed, time / diff);

            // Increases speed on slope depending on time spent on slope and slope angle
            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeDetect.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        moveSpeed = desiredSpeed;
    }
    
    // Moves the player when they are on slope/ground/air
    private void MovePlayer()
    {
        // Move direction
        moveDir = orientation.forward * vertInput + orientation.right * horzInput;

        // on a slope
        if (OnSlope() && !leavingSlope)
        {
            body.AddForce(20f * moveSpeed * GetSlopeMoveDirection(moveDir), ForceMode.Force);

            //if (body.linearVelocity.y > 0)
            //{
            body.AddForce(Vector3.down * 150f, ForceMode.Force);
            //}
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

    // Limits the player's speed unless they exceed a speed threshhold or are dashing
    private void SpeedControl()
    {
        // Limits the speed on slope
        if (OnSlope() && !leavingSlope)
        {
            if (body.linearVelocity.magnitude > moveSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * moveSpeed;
            }
        }
        
        else if (moveSpeed > moveSpeed + 3 || isDashing)
        {
            // No speed limiting
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

    // Jumps
    private void Jump()
    {
        leavingSlope = true;

        // Resets the y velocity (keeps all jumps the same)
        body.linearVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);

        body.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        lastJumpTime = Time.time;
        jumpCount--;
    }

    // Dashes
    private void Dash()
    {
        if(omniDashUnlocked)
        {
            body.AddForce(cam.transform.forward.normalized * dashForce, ForceMode.Impulse);
        }
        else
        {
            body.AddForce(moveDir.normalized * dashForce, ForceMode.Impulse);
        }
        moveSpeed = dashSpeed;
        isDashing = true;
        dashTime = maxDashTime;
        curStamina -= 50;
        staminaRechargeTimer = 0;
        StartCoroutine(ChangeDashFOV());
    }

    // Makes isDashing false if the player has been dashing for the max dash time
    private void DashTimer()
    {
        dashTime -= Time.deltaTime;
        if (dashTime <= 0)
        {
            isDashing = false;
        }
    }

    // Passively recharges the player's stamina 
    private void StaminaRecharge()
    {
        curStamina += staminaRechargeRate * Time.deltaTime;
        curStamina = Mathf.Min(curStamina, maxStamina);
    }    

    // Checks if the player is standing on a slope
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeDetect, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeDetect.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    // Gets the direction the player must move to walk parallel up the slope
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeDetect.normal).normalized;
    }
    
    //WIP
    private IEnumerator ChangeDashFOV()
    {
        float timeElapsed = 0;
        float desiredFov = 75;
        float desiredPos = horzInput > 0 ? 0.01f : horzInput < 0 ? -0.01f : 0;
        while (timeElapsed < fovChangeTime)
        {
            float t = timeElapsed / fovChangeTime;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFov, t);
            //float newPosX = Mathf.Lerp(moveDir.normalized.x, cam.transform.position.x + (moveDir.normalized.x * desiredPos), t);
            //float newPosZ = Mathf.Lerp(moveDir.normalized.z, cam.transform.position.z + (moveDir.normalized.z * desiredPos), t);
            //cam.transform.position = new Vector3(newPosX, cam.transform.position.y, newPosZ);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        cam.fieldOfView = desiredFov;
        //cam.transform.position = new Vector3(desiredPos, cam.transform.position.y, cam.transform.position.z);
        timeElapsed = 0;
        desiredFov = 60;
        //desiredPos = 0;
        while (timeElapsed < fovChangeTime)
        {
            float t = timeElapsed / fovChangeTime / 4;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFov, t);
            //float newPosX = Mathf.Lerp(moveDir.normalized.x, cam.transform.position.x + (moveDir.normalized.x * desiredPos), t);
            //float newPosZ = Mathf.Lerp(moveDir.normalized.z, cam.transform.position.z + (moveDir.normalized.z * desiredPos), t);
            //cam.transform.position = new Vector3(newPosX, cam.transform.position.y, newPosZ);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        cam.fieldOfView = 60;
        //cam.transform.position = new Vector3(body.transform.position.x, body.transform.position.y + 1.35f, body.transform.position.z);

    }

    // Unlocks the dash ability
    public void UnlockDash()
    {
        dashUnlocked = true;
        dash.Enable();
    }

    public void UnlockOmniDash()
    {
        omniDashUnlocked = true;
    }

}
