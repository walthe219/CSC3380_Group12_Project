using UnityEngine;
using System.Collections;

public class GunParticleFXs : MonoBehaviour
{
    [SerializeField] Transform gunLocation;
    [SerializeField] GameObject impactEnemyParticleSystem;
    [SerializeField] GameObject impactGenericParticleSystem;
    [SerializeField] GameObject muzzleFlashParticleSystem;
    [SerializeField] TrailRenderer bulletTrailPrefab;

    [SerializeField] float projectileDuration = 0.1f;
    [SerializeField] PlayerStats currPlayerStats;

    private void Start()
    {
        GunScript.OnBulletFired += muzzleFlash;
        GunScript.OnTargetHit += targetHit;
        GunScript.OnNonTargetHit += nonTargetHit;
        GunScript.OnMiss += onMiss;
    }

    void muzzleFlash()
    {
        //Debug.Log("Muzzle Flash");
        GameObject muzzleFlash = Instantiate(muzzleFlashParticleSystem, gunLocation);

        ParticleSystem particle = muzzleFlash.GetComponent<ParticleSystem>();
        particle.Play();
        Destroy(muzzleFlash.gameObject, particle.main.duration);

    }

    void targetHit(RaycastHit hit)
    {
        StartCoroutine(SpawnTrail(null, hit.point, hit.normal, true, true));
    }

    void nonTargetHit(RaycastHit hit)
    {
        StartCoroutine(SpawnTrail(null, hit.point, hit.normal, true, false));
    }

    void onMiss(RaycastHit hit)
    {
        StartCoroutine(SpawnTrail(null, gunLocation.position + gunLocation.forward * currPlayerStats.gunRange, Vector3.zero, false, false));
    }



    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hit, Vector3 hitNormal, bool madeImpact, bool enemyHit)
    {
        if(trail == null)
        {
            trail = Instantiate(bulletTrailPrefab, gunLocation.position, Quaternion.identity);
        }
        trail.time = projectileDuration;
        float elapsedTime = 0;
        Vector3 start = trail.transform.position;

        while (elapsedTime < 1)
        {
            trail.transform.position = Vector3.Lerp(start, hit, elapsedTime);
            elapsedTime += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit;
        if (madeImpact)
        {
            GameObject impact = Instantiate(impactGenericParticleSystem, hit, Quaternion.FromToRotation(Vector3.up, hitNormal));
            impact.GetComponent<ParticleSystem>().Play();
            Destroy(impact, 2f);

            if (enemyHit)
            {
                GameObject enemyImpact = Instantiate(impactEnemyParticleSystem, hit, Quaternion.FromToRotation(Vector3.up,hitNormal));
                enemyImpact.GetComponent<ParticleSystem>().Play();
                Destroy(enemyImpact, 2f);
            }
        }

        Destroy(trail.gameObject, trail.time);
    }
}
