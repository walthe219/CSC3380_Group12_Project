using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] Transform head;
    [SerializeField] Transform barrelEnd;
    [SerializeField] float turningSpeed;
    [SerializeField] float detectionRadius;

    [Header("Projectile")]
    [SerializeField] float fireRate;
    [SerializeField] float projSpeed;
    [SerializeField] float projRadius;
    [SerializeField] float projDamage;

    private float fireCooldown;

    void Update()
    {
        Color headColor = head.gameObject.GetComponent<Renderer>().material.color;

        Vector3 targetDirection = head.position - Camera.main.transform.position;
        if(targetDirection.magnitude <= detectionRadius)
        {
            headColor = Color.red;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, turningSpeed * Time.deltaTime);
        }
        else
        {
            headColor = Color.gray;
        }

        if (fireCooldown <= 0f)
        {
            CreateProjectile(barrelEnd.position, projRadius,projSpeed);
            fireCooldown = 1f / fireRate;
        }

        fireCooldown -= Time.deltaTime;

    }
    
    GameObject CreateProjectile(Vector3 point, float radius, float speed)
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var script = projectile.AddComponent<Projectile>();
        script.projSpeed = this.projSpeed;
        script.projDamage = this.projDamage;

        projectile.name = "Turret Projectile";
        projectile.transform.position = point;
        projectile.transform.localScale *= radius;
        projectile.GetComponent<Renderer>().material.color = Color.red;

        return projectile;
    }
    
    private class Projectile : MonoBehaviour
    {
        public float timer = 0;
        public float lifetime = 3f;

        public float projSpeed;
        public float projDamage;
        private void Update()
        {
            Transform transform = this.gameObject.transform;
            transform.Translate(transform.forward * projSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            if (timer > lifetime)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Projectile hit " + collision.gameObject.name, collision.gameObject);
            var Player = collision.gameObject.GetComponent<ControlScriptReference>();
            var PlayerHealth = collision.gameObject.GetComponent<PlayerDamageManager>();
            if (PlayerHealth != null)
            {
                
                PlayerHealth.dealDamage(projDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
