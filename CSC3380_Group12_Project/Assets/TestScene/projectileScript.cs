using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public float projectile_speed = 10;
    public float projectile_size = 5;
    public Color color;
    public float maxDuration;

    public Rigidbody rb;

    private float timer = 0;

    private void Start()
    {
        rb.AddRelativeForce(Vector3.forward * projectile_size);
        transform.localScale = new Vector3(1, 1, 1) * projectile_size;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Buttlet Collision");
    }
}
