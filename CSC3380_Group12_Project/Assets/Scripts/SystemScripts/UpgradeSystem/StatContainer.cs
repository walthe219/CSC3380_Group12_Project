using System;
using System.Collections;
using UnityEngine;

/*
 * Defines all players stats that are upgradable, superclass of PlayerStats and UpgradeData
 * To add a new upgradable player stat, declare a new field for that stat, then add a line in the change() function
 * Ex. add float gamblingLuck as field:
 *  public float gamblingLuck;
 * Then in change() add: 
 *  gamblingLuck = changeStat(gamblingLuck, other.gamblingLuck);
 *  
 *  NOTE: only excepts stats that can be represented by floats
 *  Ex. int,float,double OK      String, char, array[], Object NOT
 */
public class StatContainer: ScriptableObject
{
    public float health;
    public float stamina;
    public int ammo;
    public float moveSpeed;
    public int numJumps;
    public float damage;
    public float numRoomsComp;
   

    /*
     * Takes in another StatContainer and modfies the stats of this StatContainer based on some function applied seperately to each stat
     */
    public void change(StatContainer other, Func<float,float,float> changeStat)
    {
        health = changeStat(health, other.health);
        stamina = changeStat(stamina, other.stamina);
        ammo = (int)changeStat(ammo, other.ammo);
        moveSpeed = changeStat(moveSpeed,other.moveSpeed);
        numJumps = (int)changeStat(numJumps, other.numJumps);
        damage = changeStat(damage, other.damage);
        numRoomsComp = changeStat(numRoomsComp, other.numRoomsComp);
    }

    /*
     * Adds the stats of another StatContainer to this StatContainer's stats
     */
    public void add(StatContainer other)
    {
        change(other, (a, b) => a + b);
        //(a, b) => a + b is a lambda statement that defines a function taking in two floats a and b and returning a float a+b
    }

    /*
     * Set the stats of another StatContainer to this StatContainer's stats
     */
    public void set(StatContainer other)
    {
        change(other, (a, b) => b);
        //(a, b) => b is a lambda statement that defines a function taking in two floats a and b and returning a float b
    }

}