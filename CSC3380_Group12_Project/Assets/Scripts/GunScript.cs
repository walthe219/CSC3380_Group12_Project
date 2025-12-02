using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public MeshRenderer gun;
    public ParticleSystem muzzleFlash;
    public InputActionAsset inputActions;
    [SerializeField] PlayerStats currPlayerStats;

    private ParticleSystem impactParticleSystem;
    private TrailRenderer bulletTrail;

    //public GameObject impactEffect;

    public InputAction fireAction;

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    void Update()
    {
        if (fireAction.WasPressedThisFrame() && currPlayerStats.ammo > 0)
        {
            Shoot();
        }

    }

    void Shoot()
    {

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            //Debug.Log(hit.transform.name);

            //TrailRenderer trail = Instantiate(bulletTrail, fpsCam.transform.position, Quaternion.identity);

            //StartCoroutine(SpawnTrail(trail, hit));

            SubTarget target = hit.transform.GetComponent<SubTarget>();
            if (target != null)
            {

                target.TakeDamage(damage);

            }

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.normal));

        }

    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float elapsedTime = 0;
        Vector3 start = trail.transform.position;

        while (elapsedTime < 1)
        {
            trail.transform.position = Vector3.Lerp(start, hit.point, elapsedTime);
            elapsedTime += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}