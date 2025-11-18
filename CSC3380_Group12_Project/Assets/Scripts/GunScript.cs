using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    //public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public InputActionAsset inputActions;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats StartingStats;
    [SerializeField] PlayerStats BasePlayerStats;
    public float FireCooldown; //gun's cooldown
    public float currentFireCooldown;
    //public bool isAutomatic; if we decide to add automatic weapons
    public bool isReloading;
    public int reloadDelay;

    //public GameObject impactEffect;

    public InputAction fireAction;

    private void Start()
    {
        currentFireCooldown = FireCooldown;
        currPlayerStats.ammo = StartingStats.magSize;
        SetReloadDelayTime(3);
    }

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    void Update()
    {
        if (fireAction.WasPressedThisFrame() && currPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0 && !isReloading)
        {
            if (currentFireCooldown <= 0f) //If statement checks if cooldown has reached 0
            {
                Shoot();
                currPlayerStats.ammo--;
                currentFireCooldown = FireCooldown; //reset the current cooldown to the gun's cooldown
                Debug.Log("Resetting Firerate!");
            }
        }
        if (currentFireCooldown > 0f)
        {
            currentFireCooldown -= Time.deltaTime; //Decrements the cooldown "counter"
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading......");
        yield return new WaitForSeconds(reloadDelay);
        currPlayerStats.ammo = BasePlayerStats.magSize;
        isReloading = false;
        Debug.Log("Reloaded!");

    }

    void SetReloadDelayTime(int reloadDelay)
    {
        this.reloadDelay = reloadDelay;
    }
    void Shoot()
    {

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currPlayerStats.gunRange)) 
        {

            //Debug.Log(hit.transform.name);

            SubTarget target = hit.transform.GetComponent<SubTarget>();
            if (target != null)
            {

                target.TakeDamage(damage);

            }

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.normal));

        }

    }

    
}