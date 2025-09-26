using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController charCont;

    public float currentPlayerSpeed;
    public float playerSpeed = 15f;
    public float airDrag = 0.5f;
    private float myGravity = -10f;
    public float jumpH = 2f;

    public Transform groundCheck;
    private float groundDist = 0.4f;
    public LayerMask groundMask;
    private bool onGround;

    private float currentPlayerHeight;

    private Vector3 movementVector;
    private Vector3 myVelocity;

    //Input System
    public InputAction playerMove, playerCrouch, playerJump;


    private void OnEnable()
    {
        playerMove = InputSystem.actions.FindAction("Move");
        playerCrouch = InputSystem.actions.FindAction("Crouch");
        playerJump = InputSystem.actions.FindAction("Jump");
    }

    private void OnDisable()
    {
        playerMove.Disable();
        playerCrouch.Disable();
        playerJump.Disable();
    }

    void Start()
    {

        currentPlayerSpeed = playerSpeed;
        currentPlayerHeight = charCont.height;

    }

    void Update()
    {

        GetInput();

    }

    void GetInput()
    {

        onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (onGround && myVelocity.y < 0)
        {

            myVelocity.y = -5f;
            playerSpeed = currentPlayerSpeed;

        }

        /*float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");*/
        Vector2 tempVec = playerMove.ReadValue<Vector2>();

        movementVector = (tempVec.x * transform.right) + (tempVec.y * transform.forward);

        charCont.Move(movementVector * playerSpeed * Time.deltaTime);

        if (playerJump.WasPressedThisFrame() && onGround)
        {

            myVelocity.y = Mathf.Sqrt(jumpH * -2f * myGravity);
            playerSpeed *= airDrag;

        }

        myVelocity.y += myGravity * Time.deltaTime;
        charCont.Move(myVelocity * Time.deltaTime);

        if (playerCrouch.IsPressed())
        {

            charCont.height = currentPlayerHeight / 2;
            playerSpeed *= 0.7f;

        }
        else
        {

            charCont.height = currentPlayerHeight;
            playerSpeed = currentPlayerSpeed;

        }

    }

}