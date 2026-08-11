using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class GunScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] PlayerStats currPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    [SerializeField] Transform gunLocation;
    [SerializeField] LayerMask hitableLayers;

    [Header("State")]
    [SerializeField] bool isReloading;
    [SerializeField] bool isAutoFire;
    [SerializeField] bool isAutoReload;
    [SerializeField] float autoReloadDelay = 0.5f;

    [Header("Damage Falloff")]
    [SerializeField] float maxFalloffRangeMult = 1.25f;
    [SerializeField] float falloffDMGFloorMult = 0.0f;

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
        bool fireActivated = !isAutoFire ? fireAction.WasPressedThisFrame() : fireAction.IsPressed();
        if (fireActivated && currPlayerStats.ammo > 0 && fireDelay <= 0f)
        {
            Shoot();

            currPlayerStats.ammo--;
            if (currPlayerStats.ammo <= 0)
                OnMagazineEmpty?.Invoke();

            fireDelay = 1 / currPlayerStats.Firerate; //reset the current cooldown to the gun's cooldown
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
        //Debug.Log("Reloading......");
       
        float reloadDelay = 1/currPlayerStats.reloadSpeed;
        yield return new WaitForSeconds(reloadDelay);

        currPlayerStats.ammo = BasePlayerStats.ammo;
        isReloading = false;
        OnFinishReload?.Invoke();
        //Debug.Log("Reloaded!");
        fireDelay = 0;
    }

    void Shoot()
    {
        OnBulletFired?.Invoke();

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currPlayerStats.gunRange * maxFalloffRangeMult, hitableLayers);

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

        dealDamage(target,hit);

        
    }

    void dealDamage(SubTarget target, RaycastHit hit)
    {
        //every 100% multishot chance creates an extra shot
        int multihits = (int)(currPlayerStats.multishot);
        if (UnityEngine.Random.value < currPlayerStats.multishot % 1)
            multihits++;

        for (int i = 0; i < multihits; i++)
        {
            DamageValue damage = new DamageValue(currPlayerStats.damage, gameObject, hit.point);
            damage.addCritMult(currPlayerStats.critMult);
            damage.addCritChance(currPlayerStats.critChance);

            float falloffMult = calcFallOffMult(Vector3.Distance(hit.point, transform.position));
            damage.addDmgMult(falloffMult);

            target.TakeDamage(damage);
        }
    }

    float calcFallOffMult(float hitDist)
    {
        float cutoffDist = currPlayerStats.gunRange;
        float maxDist = currPlayerStats.gunRange * maxFalloffRangeMult;

        float multiplier = 1.0f;
        if(hitDist < cutoffDist)
        {
            multiplier = 1.0f;
        }
        else if(hitDist > maxDist)
        {
            multiplier = falloffDMGFloorMult;
        }
        //between base range and max falloff range
        else
        {
            // linear interpolation between falloff point and max falloff range
            multiplier = falloffDMGFloorMult + (hitDist - maxDist) * (falloffDMGFloorMult - 1) / (maxDist - cutoffDist);
        }

        //Debug.Log("Falloff: " + multiplier);
        return multiplier;

    }
}