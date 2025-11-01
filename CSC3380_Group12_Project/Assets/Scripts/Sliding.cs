using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{

    public Transform orientation;
    public Transform playerObj;
    private Rigidbody body;
    private NewMovement moveScript;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    private float slideYScale = 0.5f;
    private float startYScale;

    [Header("Inputs")]
    public InputAction move;
    public InputAction slide;
    private float horzInput;
    private float vertInput;

    [Header("Upgrade Unlocks")]
    [SerializeField] UpgradeManager uManager;
    [SerializeField] UpgradeData slideData;

    private void OnEnable()
    {
        move.Enable();
        slide.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        slide.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        moveScript = GetComponent<NewMovement>();
        if (InputSystem.actions)
        {
            move = InputSystem.actions.FindAction("Player/Move");
            slide = InputSystem.actions.FindAction("Player/Slide");
            OnEnable();
        }
            

        startYScale = playerObj.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        horzInput = move.ReadValue<Vector2>().x;
        vertInput = move.ReadValue<Vector2>().y;

        // Only slide if slide AND a move key is pressed
        if (slide.WasPressedThisFrame() && (horzInput != 0 || vertInput != 0))
        {
            StartSlide();
        }
        if (slide.WasReleasedThisFrame() && moveScript.isSliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (moveScript.isSliding)
        {
            SlidingMovement();
        }
    }

    // Controls all sliding movement (slope/not on slope)
    private void SlidingMovement()
    {
        Vector3 inputDir = orientation.forward * vertInput + orientation.right * horzInput;

        //Normal Sliding
        if (!moveScript.OnSlope() || body.linearVelocity.y > -0.1f)
        {
            body.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //Sliding down a slope
        else
        {
            body.AddForce(moveScript.GetSlopeMoveDirection(inputDir) * slideForce, ForceMode.Force);
        }

        // Stops the slide if the timer elapses
        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    // Shrinks the player and then starts the slide timer
    private void StartSlide()
    {
        moveScript.isSliding = true;
        slideTimer = maxSlideTime;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        body.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    // Enlarges the player
    private void StopSlide()
    {
        moveScript.isSliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
