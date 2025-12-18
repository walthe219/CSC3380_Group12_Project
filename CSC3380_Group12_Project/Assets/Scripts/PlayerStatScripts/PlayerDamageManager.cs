using System.Collections;
using UnityEngine;
using System;

//Add this script to the Player object
//This class can be used to calculate and deal damage to the player,
//defines damage methods in one script that others can call instead of each class manually applying damage to the player
//Allows for more complex player damage calculations in the future 
class PlayerDamageManager : MonoBehaviour
{
    [SerializeField] PlayerStats baseStats;
    [SerializeField] PlayerStats currentStats;

    [SerializeField] float flatFallDamage = 0;
    [SerializeField] float fallDamagePercentage = 0.10f;

    public static event Action<float> PlayerTakesDamage;

    public static PlayerDamageManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        RoomGenerator.TouchedFallPlane += dealFallPlaneDamage;
    }

    //Deals damage to player based on incoming damage, resitances or other stats can increase or decrease the damage the player takes
    // if dealTrueDamage ignores any resitances or other stats affect the damage the player takes
    public void dealDamage(float incomingDamage, bool dealTrueDamage = false)
    {
        float actualDamageTaken = incomingDamage;
        if (!dealTrueDamage)
        {
            actualDamageTaken = incomingDamage; //currently there are no stats that affects the damage the player takes, so it is the same as the incoming damage
        }
        currentStats.health -= actualDamageTaken;
        PlayerTakesDamage?.Invoke(actualDamageTaken);
    }


    //Deals a percentage of the player health as damage
    // if ignores any resitances or other stats affect the damage the player takes
    public void dealPercentageOfHealth(float percentage,bool dealTrueDamage = false, bool useMaxPlayerHealth = true)
    {
        if (useMaxPlayerHealth)
        {
            dealDamage(percentage * baseStats.health,dealTrueDamage);
        }
        else dealDamage(percentage * currentStats.health, dealTrueDamage);

    }

    //called when player touches fall plane
    void dealFallPlaneDamage(GameObject NOT_USED)
    {
        dealDamage(flatFallDamage, dealTrueDamage: true);
        dealPercentageOfHealth(fallDamagePercentage, dealTrueDamage: true, useMaxPlayerHealth: true);
    }
}