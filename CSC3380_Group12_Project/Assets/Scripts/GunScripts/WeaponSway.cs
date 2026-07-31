using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Parameters")]
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial local position of the weapon
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. Get mouse inputs
        float moveX = Input.GetAxis("Mouse X") * amount;
        float moveY = Input.GetAxis("Mouse Y") * amount;

        // 2. Calculate the target position based on input
        moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
        moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        // 3. Smoothly interpolate towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + finalPosition, Time.deltaTime * smoothAmount);
    }
}