using System;
using System.Collections;
using UnityEngine;

/*
 * Defines all players stats that are upgradable, used for PlayerStats and UpgradeData
 * To add a new upgradable player stat, create a new field for the stats, then add a line in change()
 * Ex. add float gamblingLuck as field, then in change() add: 
 * gamblingLuck = changeStat(gamblingLuck, other.gamblingLuck);
 */
public class StatContainer: ScriptableObject
{
    public float baseHealth;
    public float baseStamina;
    public int baseAmmo;
    public float moveSpeed;
    public int numJumps;
    public float damage;
   

    /*
     * Takes in another StatContainer and modfies the stats of this StatContainer based on some function applied seperately to each stat
     */
    public void change(StatContainer other, Func<float,float,float> changeStat)
    {
        baseHealth = changeStat(baseHealth, other.baseHealth);
        baseStamina = changeStat(baseStamina, other.baseStamina);
        baseAmmo = (int)changeStat(baseAmmo, other.baseAmmo);
        moveSpeed = changeStat(moveSpeed,other.moveSpeed);
        numJumps = (int)changeStat(numJumps, other.numJumps);
        damage = changeStat(damage, other.damage);
    }

    /*
     * Adds the stats of another StatContainer to this StatContainer's stats
     */
    public void add(StatContainer other)
    {
        change(other, (a, b) => a + b);
    }

    /*
     * Set the stats of another StatContainer to this StatContainer's stats
     */
    public void set(StatContainer other)
    {
        change(other, (a, b) => b);
    }

}