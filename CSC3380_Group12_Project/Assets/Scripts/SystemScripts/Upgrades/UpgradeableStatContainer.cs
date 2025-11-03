using System;
using System.Collections;
using UnityEngine;

/*
 * Defines all players stats that are UPGRADEABLE only, superclass of PlayerStats and UpgradeData
 * To add a new upgradable player stat, declare a new field for that stat, then add a line in the change() method
 * Ex. add gamblingLuck as field:
 *  public float gamblingLuck;
 * Then in change() add: 
 *  gamblingLuck = statChangeFunc(gamblingLuck, other.gamblingLuck);
 *  
 *  NOTE: only accepts stats that can be represented by floats
 *  Ex. int,float,double OK      String, char, array[], Object NOT
 */
public class UpgradeableStatContainer: ScriptableObject
{
    public float health;
    public float stamina;
    public int ammo;
    public float damage;
    public float moveSpeed;
    public int numJumps;
    public float slidePower;
    public float dashPower;

    /*
     * Changes each stat in this object by some other UpgradeAbleStat container, given some function applied to each stat
     * statChangeFunc is a function take takes in two float values, the stat values, and returns a float representing the new stat value
     */
    void change(UpgradeableStatContainer other, Func<float, float, float> statChangeFunc)
    {

        health = statChangeFunc(health, other.health);
        stamina = statChangeFunc(stamina, other.stamina);
        ammo = (int)statChangeFunc(ammo, other.ammo);
        damage = statChangeFunc(damage, other.damage);
        moveSpeed = statChangeFunc(moveSpeed, other.moveSpeed);
        numJumps = (int)statChangeFunc(numJumps, other.numJumps);
        slidePower = statChangeFunc(slidePower, other.slidePower);
        dashPower = statChangeFunc(dashPower, other.dashPower);
    }

    /*
     * Adds the stats of the other UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void add(UpgradeableStatContainer other)
    {
        change(other, (a, b) => a + b);
        //(a, b) => a + b is a lambda statement that defines a function taking in two floats a and b and returning the float float a+b
    }

    /*
     * Set the stats of the other UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void set(UpgradeableStatContainer other)
    {
        change(other, (a, b) => b);
        //(a, b) => b is a lambda statement that defines a function taking in two floats a and b and returning a float b
    }

}