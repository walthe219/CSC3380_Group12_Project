using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Rotational Recoil (Tilt)")]
    public float verticalRecoilAngle = 20f;
    public float rotationSnap = 25f;
    public float rotationReturn = 12f;

    [Header("Positional Recoil (Push Back)")]
    [Tooltip("How far back the gun kicks on the Z axis")]
    public float kickBackDistance = 0.15f;
    public float positionSnap = 30f;
    public float positionReturn = 15f;

    // Default states
    private Vector3 defaultLocalRotation;
    private Vector3 defaultLocalPosition;

    // Current & Target Rotations
    private float currentRecoilX;
    private float targetRecoilX;

    // Current & Target Positions
    private float currentKickZ;
    private float targetKickZ;

    private void OnEnable()
    {
        GunScript.OnBulletFired += Fire;
    }

    void Start()
    {
        // Store the default resting position and rotation
        defaultLocalRotation = transform.localEulerAngles;
        defaultLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. Handle Rotational Recoil
        targetRecoilX = Mathf.Lerp(targetRecoilX, 0f, rotationReturn * Time.deltaTime);
        currentRecoilX = Mathf.Lerp(currentRecoilX, targetRecoilX, rotationSnap * Time.deltaTime);
        transform.localEulerAngles = defaultLocalRotation + new Vector3(-currentRecoilX, 0f, 0f);

        // 2. Handle Positional Recoil (Push Back)
        targetKickZ = Mathf.Lerp(targetKickZ, 0f, positionReturn * Time.deltaTime);
        currentKickZ = Mathf.Lerp(currentKickZ, targetKickZ, positionSnap * Time.deltaTime);
        transform.localPosition = defaultLocalPosition + new Vector3(0f, 0f, -currentKickZ);
    }

    public void Fire()
    {
        // Trigger both rotation and position changes instantly
        targetRecoilX += verticalRecoilAngle;
        targetKickZ += kickBackDistance;
    }
}
