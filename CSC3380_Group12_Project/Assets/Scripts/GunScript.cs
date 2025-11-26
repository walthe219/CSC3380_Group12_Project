using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    public GameObject relaod_icon;
    public bool isReloading;
    public float reloadDelay; //variable to set reload speed manully

    //public GameObject impactEffect;

    public InputAction fireAction;

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    void Update()
    {
        //NOTE: gunrange and damage work as intended as of 11/25/25

        if (fireAction.WasPressedThisFrame() && currPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0 && !isReloading)
        {

            if (currPlayerStats.Firerate <= 0f) //If statement checks if cooldown has reached 0
            {
                
                Shoot();
                currPlayerStats.ammo--;
                currPlayerStats.Firerate = BasePlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
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

        if (currPlayerStats.ammo == 0 && !isReloading) {
            StartCoroutine(Reload());
        }

        //Stat Updater Section
        //Since parameters dont like to take in "currplayerstats.blah" I am updating these values every frame
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading......");
        relaod_icon.SetActive(true);
        reloadDelay = currPlayerStats.reloadSpeed;
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

            SubTarget target = hit.transform.GetComponent<SubTarget>();
            if (target != null)
            {

                target.TakeDamage(currPlayerStats.damage);

            }

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.normal));

        }

    }

    
}