using System;
using System.Collections;
using UnityEngine;

/*
 * Defines all players stats that are upgradable, superclass of PlayerStats and UpgradeData
 * To add a new upgradable player stat, declare a new field for that stat, then add a line in the change() function
 * Ex. add gamblingLuck as field:
 *  public float gamblingLuck;
 * Then in change() add: 
 *  gamblingLuck = changeStat(gamblingLuck, other.gamblingLuck);
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
     * Takes in another UpgradeableStatContainer and modfies the stats of this UpgradeableStatContainer based on some function 
     * Applies a function to each stat
     */
    public void change(UpgradeableStatContainer other, Func<float, float, float> changeFunc)
    {
        health = changeFunc(health, other.health);
        stamina = changeFunc(stamina, other.stamina);
        ammo = (int)changeFunc(ammo, other.ammo);
        damage = changeFunc(damage, other.damage);
        moveSpeed = changeFunc(moveSpeed, other.moveSpeed);
        numJumps = (int)changeFunc(numJumps, other.numJumps);
        slidePower = changeFunc(slidePower, other.slidePower);
        dashPower = changeFunc(dashPower, other.dashPower);
    }

    /*
     * Adds the stats of another UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void add(UpgradeableStatContainer other)
    {
        change(other, (a, b) => a + b);
        //(a, b) => a + b is a lambda statement that defines a function taking in two floats a and b and returning a float a+b
    }

    /*
     * Set the stats of another UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void set(UpgradeableStatContainer other)
    {
        change(other, (a, b) => b);
        //(a, b) => b is a lambda statement that defines a function taking in two floats a and b and returning a float b
    }

}