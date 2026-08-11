using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletChaining : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] TrailRenderer bulletTrailPrefab;

    [Header("Chaining")]
    [SerializeField] bool chainingUnlocked;
    [SerializeField] float chainingRadius;
    [SerializeField] float chainingDamageMult;
    [SerializeField] float chainChance;
    [SerializeField] float maxChains;
    [SerializeField] AudioClip[] riocochetSounds;

    [Header("Restrictions")]
    [SerializeField] bool enforceChance = true;
    [SerializeField] bool enforceLOS = true;
    [SerializeField] bool enforceEnemy = true;
    [SerializeField] bool enforceMaxChains = true;


    public void Start()
    {
        GunScript.OnTargetHit += StartBulletChain;
        UnlockFunctions.UnlockBulletChaining += () => chainingUnlocked = true;
    }

    void StartBulletChain(RaycastHit hit)
    {
        if (!chainingUnlocked) 
            return;

        if (enforceEnemy && hit.transform.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        if (enforceChance && Random.Range(0f, 1f) > chainChance) 
            return;
       
        Target originalTarget = hit.transform.GetComponent<SubTarget>().target;
        HashSet<Target>  chained = new HashSet<Target>();

        BulletChain(originalTarget, chained);
    }

    void BulletChain(Target originalTarget, HashSet<Target> chained)
    {
        chained.Add(originalTarget);

        if (enforceMaxChains && chained.Count > maxChains) 
            return;

        Vector3 originalTargetPos = originalTarget.transform.position;

        Collider[] potenialChainingTargts = Physics.OverlapSphere(originalTargetPos, chainingRadius, LayerMask.GetMask("Enemy"));
        Debug.Log(ArrayHelper.print(potenialChainingTargts));

        HashSet<Target> examinedTargets = new HashSet<Target>();
        examinedTargets.Add(originalTarget); //bullet cant chain to original target hit

        float closetDistance = float.MaxValue;
        Target closestEnemy = null;
        Vector3 cloestEnemyPos = Vector3.zero;

        foreach (Collider possibility in potenialChainingTargts)
        {
            var sub = possibility.transform.GetComponent<SubTarget>();
            if (!sub) continue;

            Target potentialTarget = sub.target;
            Vector3 potentialTargetPos = potentialTarget.transform.position;

            if (examinedTargets.Contains(potentialTarget) || chained.Contains(potentialTarget))
                continue;

            float objDist = Vector3.Distance(originalTargetPos, potentialTargetPos);
            Vector3 objDir = potentialTargetPos - originalTargetPos;

            // check Line of sight 
            if (enforceLOS && !Physics.Raycast(originalTargetPos +objDir * 0.5f , objDir, objDist))
            {
                continue;
            }

            if (objDist < closetDistance)
            {
                closetDistance = objDist;
                closestEnemy = potentialTarget;
                cloestEnemyPos = potentialTargetPos;
            }

            examinedTargets.Add(potentialTarget);
        }


        if (closestEnemy == null)
        {
            //Debug.Log("No chain target in range");
            return;
        }



        DamageValue damage = new DamageValue(currPlayerStats.damage * chainingDamageMult, gameObject, cloestEnemyPos);
        closestEnemy.TakeDamage(damage);


        int random = Random.Range(0, riocochetSounds.Length);
        SoundFXManager.instance.PlaySoundFXClip(riocochetSounds[random], transform, 1f);

        StartCoroutine(SpawnTrail(originalTargetPos, cloestEnemyPos));

        BulletChain(closestEnemy, chained);
    }


    private IEnumerator SpawnTrail(Vector3 startPos, Vector3 endPos)
    {
        TrailRenderer trail = Instantiate(bulletTrailPrefab, startPos, Quaternion.identity);
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime / trail.time;
            yield return null;
        }

        Destroy(trail.gameObject, trail.time);
    }
}
