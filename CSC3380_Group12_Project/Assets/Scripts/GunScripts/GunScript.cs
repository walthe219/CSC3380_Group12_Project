using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class GunScript : MonoBehaviour
{
    [SerializeField] Camera fpsCam;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    [SerializeField] Transform gunLocation;

    [Header("State")]
    [SerializeField] bool isReloading;
    [SerializeField] float reloadDelay;
    [SerializeField] bool isAuto;

    [Header("Unlocks")]
    [SerializeField] bool AutoUnlocked;
    [SerializeField] bool isNotAutoReload;

    [Header("Inputs")]
    [SerializeField] InputAction fireAction;
    [SerializeField] InputAction reloadAction;
    [SerializeField] InputAction toggleAutoFireAction;

    //Static Gun Events
    public static event Action OnTriggerPull;
    public static event Action OnBulletFired;

    public static event Action<RaycastHit> OnMiss;
    public static event Action<RaycastHit> OnAnythingHit;
    public static event Action<RaycastHit> OnTargetHit;
    public static event Action<float> OnDamageDelt;
    public static event Action<RaycastHit> OnNonTargetHit;

    public static event Action OnMagazineEmpty;
    public static event Action OnStartReload;
    public static event Action OnFinishReload;
    public static event Action<bool> OnToggleAutoFire;


    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
        reloadAction = InputSystem.actions.FindAction("Reload");
        toggleAutoFireAction = InputSystem.actions.FindAction("Toggle Automatic Fire");

        UnlockFunctions.UnlockAutoFireEvent += unlockAutoFire;
    }

    [ContextMenu("unlockAutoFire")]
    public void unlockAutoFire() {
        AutoUnlocked = true;
        toggleAutoFire();
    }

    [ContextMenu("toggleAutoFire")]
    public void toggleAutoFire() { 
        isAuto = !isAuto;
        OnToggleAutoFire?.Invoke(isAuto);
        if (isAuto)
        {
            Debug.Log("Automatic was toggled on");
        }
        if (!isAuto)
        {
            Debug.Log("Automatic was toggled off");
        }
    }

    public void ToggleAutoReload()
    {
        isNotAutoReload = !isNotAutoReload;
    }

    private void Start()
    {
        AutoUnlocked = false;
    }

    void Update()
    {
        if((fireAction.WasPressedThisFrame() && !isAuto) || (fireAction.IsPressed() && isAuto))
        {
            if (currPlayerStats.ammo > 0 && !PauseMenu1.GameIsPaused && Time.timeScale > 0 && !isReloading )
            {
                if (currPlayerStats.Firerate <= 0f) //checks if cooldown has reached 0
                {
                    Shoot();

                    currPlayerStats.ammo--;
                    if (currPlayerStats.ammo <= 0)
                        OnMagazineEmpty?.Invoke();

                    currPlayerStats.Firerate = 1 / BasePlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
                    //Debug.Log("Resetting Firerate!");
                }
            }
        }

        if (currPlayerStats.Firerate > 0f)
        {
            currPlayerStats.Firerate -= Time.deltaTime; //Decrements the cooldown "counter"
        }
        
        if (reloadAction.WasPressedThisFrame() && !isReloading && (currPlayerStats.ammo < BasePlayerStats.ammo))
        {
            if (currPlayerStats.ammo != BasePlayerStats.ammo)
            {
                StartCoroutine(Reload());
            }
        }

        if (currPlayerStats.ammo == 0 && !isReloading && !isNotAutoReload) { 
            StartCoroutine(Reload());
        }

        if (toggleAutoFireAction.WasPressedThisFrame() && AutoUnlocked)
        {
            toggleAutoFire();
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading......");
        OnStartReload?.Invoke();
        reloadDelay = 1/currPlayerStats.reloadSpeed;
        yield return new WaitForSeconds(reloadDelay);
        currPlayerStats.ammo = BasePlayerStats.ammo;
        isReloading = false;
        OnFinishReload?.Invoke();
        Debug.Log("Reloaded!");
    }

    void Shoot()
    {
        OnBulletFired?.Invoke();

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currPlayerStats.gunRange)) //if bullet hits anything
        {
            //Debug.Log(hit.transform.name);
            OnAnythingHit?.Invoke(hit);

            SubTarget target = hit.transform.GetComponent<SubTarget>();

            if (target != null) //target hit
            {
                Debug.Log("You hit the target " + target.gameObject.name);
                OnTargetHit?.Invoke(hit);

                //every 100% multishot chance guarentes an extra shot
                int multihits = (int)(currPlayerStats.multishot);
                if (UnityEngine.Random.value < currPlayerStats.multishot % 1)
                    multihits++;

                for(int i = 0; i< multihits; i++)
                {
                    float baseDamage = currPlayerStats.damage;

                    //every 100% crit chance guarentes a crit
                    int numCrits = (int)(currPlayerStats.critChance);
                    if (UnityEngine.Random.value < currPlayerStats.critChance % 1)
                        numCrits++;

                    float dmgMult = 1 + numCrits * (currPlayerStats.critMult - 1);

                    Debug.Log("You did damage with " + numCrits + " num crits");
                    target.TakeDamage(baseDamage * dmgMult, hit.point);
                }
            }
            else //non target hit
            {
                Debug.Log("You hit something other than the target!");
                OnNonTargetHit?.Invoke(hit);
            }

        }
        else { //If nothing hit
            Debug.Log("You completely missed lmao");
            OnMiss?.Invoke(hit);
        }

    }
}