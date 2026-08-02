using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] Transform head;
    [SerializeField] Transform barrelEnd;
    [SerializeField] float turningSpeed;
    [SerializeField] float detectionRadius;

    [Header("Projectile")]
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] float fireRate;
    [SerializeField] float projSpeed;
    [SerializeField] float projRadius;
    [SerializeField] float projDamage;

    private float fireCooldown;

    void Update()
    {
        Material headMaterial = head.gameObject.GetComponent<Renderer>().material;

        Vector3 targetDirection = Camera.main.transform.position - head.position;

        //need to do LoS check aswell
        if(targetDirection.magnitude <= detectionRadius)
        {
            headMaterial.SetColor("_BaseColor", Color.yellow);

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, turningSpeed * Time.deltaTime);

            if (fireCooldown <= 0f)
            {
                CreateProjectile(barrelEnd, projRadius, projSpeed);
                fireCooldown = 1f / fireRate;
            }

            fireCooldown -= Time.deltaTime;
        }
        else
        {
            headMaterial.SetColor("_BaseColor", Color.gray);
        }

        

    }
    
    GameObject CreateProjectile(Transform spawnPoint, float radius, float speed)
    {
        GameObject projectile =  Instantiate(ProjectilePrefab, transform);
        var script = projectile.AddComponent<EnemyProjectile>();
        script.projSpeed = this.projSpeed;
        script.projDamage = this.projDamage;

        projectile.name = "Turret Projectile";
        projectile.transform.position = spawnPoint.position;
        projectile.transform.rotation = spawnPoint.rotation;

        projectile.transform.localScale *= radius;
        projectile.GetComponent<Renderer>().material.color = Color.red;

        return projectile;
    }   
}
