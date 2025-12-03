using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public Camera fpsCam;
    public MeshRenderer gun;
    public ParticleSystem muzzleFlash;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    public GameObject relaod_icon;
    public GameObject SemiToggle_icon;
    public GameObject AutoToggle_icon;
    public bool isReloading;
    public float reloadDelay; //variable to set reload speed manully
    public bool isAuto;
    public bool AutoUnlocked;
    public bool LifeStealUnlocked;
    public bool isNotAutoReload;

    private ParticleSystem impactParticleSystem;
    public GameObject bulletTrailPrefab;

    //public GameObject impactEffect;

    public InputAction fireAction;

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
        UnlockFunctions.UnlockAutoFireEvent += unlockAutoFire;
        UnlockFunctions.UnlockLifeStealEvent += unlockLifeSteal;
    }


    public void unlockAutoFire() {
        AutoUnlocked = true;
        SemiToggle_icon.SetActive(true);
    }

    public void unlockLifeSteal()
    {
        LifeStealUnlocked = true;
    }

    public void toggleAutoFire() { 
        isAuto = !isAuto;
        if (isAuto)
        {
            Debug.Log("Automatic was toggled on");
            SemiToggle_icon.SetActive(false);
            AutoToggle_icon.SetActive(true);
        }
        if (!isAuto)
        {
            Debug.Log("Automatic was toggled off");
            SemiToggle_icon.SetActive(true);
            AutoToggle_icon.SetActive(false);
        }

    }

    public void ToggleAutoReload()
    {
        isNotAutoReload = !isNotAutoReload;

    }

    void Update()
    {
        //NOTE: gunrange and damage work as intended as of 11/25/25

        if (fireAction.WasPressedThisFrame() && currPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0 && !isReloading && !isAuto)
        {

            if (currPlayerStats.Firerate <= 0f) //If statement checks if cooldown has reached 0
            {
                
                Shoot();
                currPlayerStats.ammo--;
                currPlayerStats.Firerate = 1/BasePlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
                Debug.Log("Resetting Firerate!");
            }
        }

        if (fireAction.IsPressed() && currPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0 && !isReloading && AutoUnlocked && isAuto) { //AutoShoot
            if (currPlayerStats.Firerate <= 0f) //If statement checks if cooldown has reached 0
            {

                Shoot();
                currPlayerStats.ammo--;
                currPlayerStats.Firerate = 1/BasePlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
                Debug.Log("Resetting Firerate!");
            }
        }

        if (currPlayerStats.Firerate > 0f)
        {
            currPlayerStats.Firerate -= Time.deltaTime; //Decrements the cooldown "counter"
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (currPlayerStats.ammo < BasePlayerStats.ammo))
        {
            if (currPlayerStats.ammo != BasePlayerStats.ammo)
            {
                StartCoroutine(Reload());
            }
        }

        if (currPlayerStats.ammo == 0 && !isReloading && !isNotAutoReload) { 
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.T) && AutoUnlocked)
        {
            toggleAutoFire();
        }

    }

    private void Start()
    {
        /*
        AutoUnlocked = true;
        isAuto = true;
        Automatic firing and togglign works as intended when both are true
        */

        AutoUnlocked = false; //Only turns truew when player completes room with unlockfireauto upgrade, then the event triggers, and the subscriber function in this script sets 
        //AutoUnlocked to true
        LifeStealUnlocked = false; //Remember to set to false
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading......");
        relaod_icon.SetActive(true);
        reloadDelay = 1/currPlayerStats.reloadSpeed;
        yield return new WaitForSeconds(reloadDelay);
        currPlayerStats.ammo = BasePlayerStats.ammo;
        isReloading = false;
        relaod_icon.SetActive(false);
        Debug.Log("Reloaded!");
    }

    /*void SetReloadDelayTime() //Set reload speed manually
    {
        BasePlayerStats.reloadSpeed = reloadDelay;
    }*/
    void Shoot()
    {

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currPlayerStats.gunRange)) //chasnge to currplayter stats gunrange
        {

            //Debug.Log(hit.transform.name);

            TrailRenderer trail = Instantiate(bulletTrailPrefab, fpsCam.transform.position, Quaternion.identity).GetComponent<TrailRenderer>();

            StartCoroutine(SpawnTrail(trail, hit));

            SubTarget target = hit.transform.GetComponent<SubTarget>();
            if (target != null)
            {

                target.TakeDamage(currPlayerStats.damage);
                if (LifeStealUnlocked && currPlayerStats.health < BasePlayerStats.health) {
                    currPlayerStats.health = (float)(currPlayerStats.health + (currPlayerStats.damage * 0.10)); //If LifeSteal Upgrade is unlocked, then whena player successfully
                    //hits an enemy they gain a percentage of the damage they deal to their health
                    //Otherwise, if they miss they lose that percentage of health
                }

            }
            else
            {
                Debug.Log("You hit something other than the target!");
                if (LifeStealUnlocked && currPlayerStats.health < BasePlayerStats.health)
                {
                    currPlayerStats.health = (float)(currPlayerStats.health - (currPlayerStats.damage * 0.10)); //Where the player loses the percentage of health if they miss
                }
            }

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.normal));

        }
        else {
            Debug.Log("You completely missed lmao");
            if (LifeStealUnlocked && currPlayerStats.health < BasePlayerStats.health) {
                currPlayerStats.health = (float)(currPlayerStats.health - (currPlayerStats.damage * 0.10)); //Where the player loses the percentage of health if they miss
            }
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
        //Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}