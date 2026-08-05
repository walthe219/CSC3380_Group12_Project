using UnityEngine;
using System.Collections.Generic;


// This class is a MonoBehaviour attached to the gun that spawns an AoE explosion on hit that damages enemies within its radius
// This script is called when the OnAnythingHit even in GunScript is invoked
public class ExplosiveRouds : MonoBehaviour
{
    [SerializeField] bool ExplosiveRoundsUnlocked = false;

    [Tooltip("What percentage of the player damage stat does the explosion do")]
    [SerializeField, Range(0f, 1f)] float explosionDamageModifier;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] PlayerStats currPlayerStats;

    private void Start()
    {
        GunScript.OnAnythingHit += spawnExplosion;
        UnlockFunctions.UnlockExplosiveRounds += unlockExplosiveRounds;
    }

    [ContextMenu("unlockExplosiveRounds")]
    public void unlockExplosiveRounds()
    {
        ExplosiveRoundsUnlocked = true;
    }

    void spawnExplosion(RaycastHit hit)
    {
        if (!ExplosiveRoundsUnlocked)
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(hit.point, explosionRadius, LayerMask.GetMask("Enemy"));
        //Debug.Log(ArrayHelper.print(hits));

        GameObject explosionEffect = Instantiate(explosionPrefab, hit.point, Quaternion.identity);
        
        HashSet<Target> targets =  new HashSet<Target>();
        foreach (Collider obj in hits)
        {
            SubTarget sub = obj.transform.GetComponent<SubTarget>();

            if (sub != null) //target hit
            {
                Target target = sub.target;
                if (targets.Contains(target))
                {
                    continue;
                }
                var damage = new DamageValue(currPlayerStats.damage, gameObject, target.transform.position);
                damage.addDmgMult(explosionDamageModifier);
                target.TakeDamage(damage);
                targets.Add(sub.target);
                //Debug.Log("Explosion hit " + target.ToString

            }
        } 
  
    }
}
