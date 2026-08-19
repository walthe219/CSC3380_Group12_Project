using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour
{
    [Header("Grendade")]
    [SerializeField] [Min(0)] float gravityMultiplier;
    [SerializeField] AudioClip throwSound; 
    [SerializeField] AudioClip fuseSound;

    [Header("Explosion")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField][Min(0)] float explosionDamage;
    [SerializeField][Min(0)] float explosionRadius;
    [SerializeField] [Min(0)] float fuseDuration;

    private Rigidbody rb;
    private bool disabled;
    private AudioSource fuseSource;
    private AudioSource throwSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Target>().OnDeath += explode;

        if (fuseSound)
        {
            fuseSource = SoundFXManager.instance.PlaySoundFXClip(fuseSound, transform, 1f);
            fuseSource.gameObject.transform.parent = gameObject.transform;
        }
            
        if (throwSound)
        {
            throwSource = SoundFXManager.instance.PlaySoundFXClip(throwSound, transform, 1f);
            throwSource.gameObject.transform.parent = gameObject.transform;
        }
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
        StartCoroutine(StartFuse(fuseDuration));
    }

    void applyVelocity(Vector3 velocity)
    {
        rb.linearVelocity += velocity;
    }

    void explode()
    {
        Explosion.spawn(explosionPrefab, transform.position, explosionDamage, explosionRadius);
        Destroy(gameObject);
    }

    IEnumerator StartFuse(float fuseDuration)
    {
        if(fuseSound != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(fuseSound, transform, 1f);
        }
            
        yield return new WaitForSeconds(fuseDuration);
        explode();
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
