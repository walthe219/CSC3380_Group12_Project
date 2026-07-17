using UnityEngine;
using System.Collections;

public class GunParticleFXs : MonoBehaviour
{
    [SerializeField] Transform gunLocation;
    [SerializeField] ParticleSystem impactEnemyParticleSystem;
    [SerializeField] ParticleSystem impactGenericParticleSystem;
    [SerializeField] ParticleSystem muzzleFlashParticleSystem;
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
        ParticleSystem muzzleFlash = Instantiate(muzzleFlashParticleSystem, gunLocation.position, gunLocation.rotation);
        muzzleFlash.transform.parent = gunLocation;
        muzzleFlash.transform.localScale = Vector3.one;

        muzzleFlash.Play();
        Destroy(muzzleFlash.gameObject, muzzleFlash.main.duration);

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
        StartCoroutine(SpawnTrail(null, gunLocation.position + transform.forward * currPlayerStats.gunRange, Vector3.zero, false, false));
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
            if (enemyHit)
            {
                Instantiate(impactEnemyParticleSystem, hit, Quaternion.LookRotation(hitNormal));
                //SoundFXManager.instance.PlayRandomSoundFXClip(gunHitEnemySounds, transform, 1f);
            }
            else
            {
                Instantiate(impactGenericParticleSystem, hit, Quaternion.LookRotation(hitNormal));
            }
        }

        Destroy(trail.gameObject, trail.time);
    }
}
