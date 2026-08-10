using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDamage;

    public static GameObject spawn(GameObject prefab, Vector3 pos,float damage, float radius)
    {
        GameObject explosion = Instantiate(prefab, pos, Quaternion.identity);

        var script = explosion.GetComponent<Explosion>();
        if(script == null)
            script = explosion.AddComponent<Explosion>();

        script.explosionDamage = damage;
        script.explosionRadius = radius;

        // object scale = diameter so set it to twice the radius
        explosion.transform.localScale = Vector3.one * radius * 2;

        return explosion;
    }

    void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));

        HashSet<Target> alreadyHit = new HashSet<Target>();
        foreach (Collider obj in hits)
        {
            SubTarget sub = obj.transform.GetComponent<SubTarget>();

            if (sub == null) continue; //non target hit
            
            Target target = sub.target;
            if (alreadyHit.Contains(target)) continue;
            

            var damage = new DamageValue(explosionDamage, gameObject, target.transform.position);
            target.TakeDamage(damage);

            alreadyHit.Add(sub.target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }


}
