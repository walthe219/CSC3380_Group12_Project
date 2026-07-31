using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRigidbody;

    [Header("Bobbing Settings")]
    public float bobSpeedMultiplier = 2.5f;
    public float bobAmount = 0.04f;
    public float lerpSpeed = 10f;
    public float speedThreshold = 0.1f;

    private float timer = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        // Get the player's current speed along the ground (ignoring gravity/jumping)
        Vector3 horizontalVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // Check if the player is actively moving
        if (currentSpeed > speedThreshold)
        {
            // Advance the timer based on the player's physical speed
            timer += Time.deltaTime * currentSpeed * bobSpeedMultiplier;

            // Generate standard bob vectors
            float horizontalOffset = Mathf.Sin(timer) * bobAmount;
            // Y moves at double speed to create a natural dipping motion
            float verticalOffset = Mathf.Abs(Mathf.Sin(timer * 2f)) * bobAmount;

            Vector3 targetPosition = new Vector3(
                startPos.x + horizontalOffset,
                startPos.y + verticalOffset,
                startPos.z
            );

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
        }
        else
        {
            // Smoothly reset back to the baseline position when stopped
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * lerpSpeed);
        }
    }
}
