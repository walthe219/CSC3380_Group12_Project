using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    //public GameObject impactEffect;

    public InputAction fireAction;

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (fireAction.WasPressedThisFrame())
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

            Debug.Log(hit.transform.name);

            /*Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {

                target.TakeDamage(damage);

            }*/

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.normal));

        }

    }
}