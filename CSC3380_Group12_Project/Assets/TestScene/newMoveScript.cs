using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class newMoveScript : MonoBehaviour
{
    private CharacterController controller;

    public float moveSpeed = 5f;
    public float jumpStr = 8f;
    public float gravity = 20f;

    InputAction move;
    InputAction jump;

    [Header("Velocity Values")]
    [SerializeField] Vector3 xMove = Vector3.zero;
    [SerializeField] Vector3 yMove = Vector3.zero;
    [SerializeField] Vector3 zMove = Vector3.zero;

    [SerializeField]float yVelocity = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        //Movement Calculations
        Vector2 moveValue = move.ReadValue<Vector2>();
        xMove = transform.right * moveValue.x * moveSpeed * Time.fixedDeltaTime;
        zMove = transform.forward * moveValue.y * moveSpeed * Time.fixedDeltaTime;

        //Jump Calculations
        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (jump.IsPressed())
            {
                yVelocity = jumpStr;
            }
        }
        else
        {
            yVelocity -= gravity *Time.deltaTime;
            
        }
        yMove = transform.up * yVelocity;

        controller.Move((xMove + yMove + zMove));
    }
  
}
