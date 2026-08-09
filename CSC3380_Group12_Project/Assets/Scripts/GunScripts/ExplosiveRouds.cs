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
        if (!ExplosiveRoundsUnlocked) return;

        float explosionDamage = currPlayerStats.damage * explosionDamageModifier;
        GameObject explosion = Explosion.spawn(explosionPrefab, hit.point, explosionDamage, explosionRadius);
    }
}
