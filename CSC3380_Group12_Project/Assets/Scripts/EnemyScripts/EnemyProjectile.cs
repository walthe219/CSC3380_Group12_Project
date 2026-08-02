using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float timer = 0;
    public float lifetime = 3f;

    public float projSpeed;
    public float projDamage;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * projSpeed;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit " + other.gameObject.name, other.gameObject);
        var PlayerHealth = other.GetComponentInParent<PlayerDamageManager>();
        if (PlayerHealth != null)
        {
            PlayerHealth.dealDamage(projDamage);
        }
        Destroy(this.gameObject);
    }
}
