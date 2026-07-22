using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class GunScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    [SerializeField] Transform gunLocation;

    [Header("State")]
    [SerializeField] bool isReloading;
    [SerializeField] bool isAutoFire;
    [SerializeField] bool isAutoReload;
    [SerializeField] float autoReloadDelay = 0.5f;

    [Header("Unlocks")]
    [SerializeField] bool AutoFireUnlocked;

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

    private float fireDelay = 0;


    private void OnEnable()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
        reloadAction = InputSystem.actions.FindAction("Reload");
        toggleAutoFireAction = InputSystem.actions.FindAction("Toggle Automatic Fire");

        UnlockFunctions.UnlockAutoFireEvent += unlockAutoFire;
    }

    [ContextMenu("unlockAutoFire")]
    public void unlockAutoFire() {
        AutoFireUnlocked = true;
        toggleAutoFire();
    }

    [ContextMenu("toggleAutoFire")]
    public void toggleAutoFire() {
        isAutoFire = !isAutoFire;
        OnToggleAutoFire?.Invoke(isAutoFire);
        if (isAutoFire)
        {
            Debug.Log("Automatic was toggled on");
        }
        if (!isAutoFire)
        {
            Debug.Log("Automatic was toggled off");
        }
    }

    void Update()
    {
        //toggle between semi and fully automatic-fire modes if unlocked 
        if (toggleAutoFireAction.WasPressedThisFrame() && AutoFireUnlocked)
        {
            toggleAutoFire();
        }

        // cannot shoot or reload while reloading
        if (isReloading)
            return;

        // check if fire pressed or held based on curret fire mode
        bool fireActuvated = !isAutoFire ? fireAction.WasPressedThisFrame() : fireAction.IsPressed();
        if (fireActuvated && currPlayerStats.ammo > 0 && fireDelay <= 0f)
        {
            Shoot();

            currPlayerStats.ammo--;
            if (currPlayerStats.ammo <= 0)
                OnMagazineEmpty?.Invoke();

            fireDelay = 1 / BasePlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
            //Debug.Log("Resetting Firerate!");
        }
        
        // decrement firerate timer
        if (fireDelay > 0f)
        {
            fireDelay -= Time.deltaTime;
        }

        // Can only reload when mag not fully
        if (reloadAction.WasPressedThisFrame()  && (currPlayerStats.ammo < BasePlayerStats.ammo))
        {
            StartCoroutine(Reload());
        }

        // if automatic reload enabled, then reload for the player on empty mag
        if (currPlayerStats.ammo == 0 && isAutoReload)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        OnStartReload?.Invoke();
        Debug.Log("Reloading......");
       
        float reloadDelay = 1/currPlayerStats.reloadSpeed;
        yield return new WaitForSeconds(reloadDelay);

        currPlayerStats.ammo = BasePlayerStats.ammo;
        isReloading = false;
        OnFinishReload?.Invoke();
        Debug.Log("Reloaded!");
        fireDelay = 0;
    }

    void Shoot()
    {
        OnBulletFired?.Invoke();

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currPlayerStats.gunRange);

        if (!hitSomething)
        {
            Debug.Log("You completely missed lmao");
            OnMiss?.Invoke(hit);
            return;
        }

        OnAnythingHit?.Invoke(hit);

        SubTarget target = hit.transform.GetComponent<SubTarget>();

        if (target == null)
        {
            Debug.Log("You hit something other than the target!");
            OnNonTargetHit?.Invoke(hit);
            return;
        }

        Debug.Log("You hit the target " + target.gameObject.name);
        OnTargetHit?.Invoke(hit);

        //every 100% multishot chance creates an extra shot
        int multihits = (int)(currPlayerStats.multishot);
        if (UnityEngine.Random.value < currPlayerStats.multishot % 1)
            multihits++;

        for (int i = 0; i < multihits; i++)
        {
            float baseDamage = currPlayerStats.damage;

            //every 100% crit chance guarentes a crit
            int numCrits = (int)(currPlayerStats.critChance);
            if (UnityEngine.Random.value < currPlayerStats.critChance % 1)
                numCrits++;

            // total critMult is base critMult times number of crits
            float dmgMult = 1 + numCrits * (currPlayerStats.critMult - 1);

            Debug.Log("You did damage with " + numCrits + " num crits");
            target.TakeDamage(baseDamage * dmgMult, hit.point);
        }
    }
}