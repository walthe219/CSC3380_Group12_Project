using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class GrenadeScript : MonoBehaviour
{
    [Header("Grendade")]
    [SerializeField] [Min(0)] float gravityMultiplier;

    [Header("Explosion")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField][Min(0)] float explosionDamage;
    [SerializeField][Min(0)] float explosionRadius;

    private Rigidbody rb;
    private bool disabled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Target>().OnDeath += explode;
    }

    private void FixedUpdate()
    { 
        Vector3 customGravity = Physics.gravity * gravityMultiplier;
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (disabled) return;

        disablePhysics();
    }

    void applyVelocity(Vector3 velocity)
    {
        rb.linearVelocity += velocity;
    }

    void explode()
    {
        Explosion.spawn(explosionPrefab, transform.position, explosionDamage, explosionRadius);
    }

    void disablePhysics()
    {
        rb.linearVelocity = Vector3.zero;
        rb.interpolation = RigidbodyInterpolation.None;
        rb.isKinematic = true;
        rb.freezeRotation = true;

        disabled = true;
    }

    void enablePhysics()
    {
        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.WakeUp();
    }
}
