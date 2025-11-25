using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage;
    public float range;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public InputActionAsset inputActions;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats StartingStats;
    [SerializeField] PlayerStats BasePlayerStats;
    public GameObject relaod_icon;
    public float Firerate; //gun's cooldown
    public float currentFireCooldown;
    //public bool isAutomatic; if we decide to add automatic weapons
    public bool isReloading;
    public float reloadDelay; //variable to set reload speed manully

    //public GameObject impactEffect;

    public InputAction fireAction;

    private void Start()
    {
        currentFireCooldown = Firerate;
        currPlayerStats.ammo = currPlayerStats.magSize;
        currPlayerStats.magSize = BasePlayerStats.magSize;
        currPlayerStats.gunRange = BasePlayerStats.gunRange;
        currPlayerStats.reloadSpeed = BasePlayerStats.reloadSpeed;
        currPlayerStats.Firerate = BasePlayerStats.Firerate;
        currPlayerStats.damage = BasePlayerStats.damage;
        damage = currPlayerStats.damage; //Damage for takedamage function since weird behavior is encountered when just currplayerstats.blah is inpuit as parameter
        range = currPlayerStats.gunRange; //^^ same here
    }

    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    void Update()
    {

        if (currPlayerStats.damage != damage) { //If dmaage is not equal to current damage update damage
            damage = currPlayerStats.damage;
        }

        if (currPlayerStats.gunRange != range) { //If gunrange is not equal to current gunrange update gunrange
            range = currPlayerStats.gunRange;
        }

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
        
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (currPlayerStats.ammo < BasePlayerStats.magSize))
        {
            if (currPlayerStats.ammo != BasePlayerStats.magSize)
            {
                StartCoroutine(Reload());
            }
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
        currPlayerStats.ammo = BasePlayerStats.magSize;
        isReloading = false;
        relaod_icon.SetActive(false);
        Debug.Log("Reloaded!");

    }

    void SetReloadDelayTime() //Set reload speed manually
    {
        BasePlayerStats.reloadSpeed = reloadDelay;
    }
    void Shoot()
    {

        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) //chasnge to currplayter stats gunrange
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