using System;
using System.Collections.Generic; 
using UnityEngine;

/*
 * Defines all players stats that are UPGRADEABLE only, superclass of PlayerStats and UpgradeData
 * To add a new upgradable player stat, declare a new field for that stat, then add a line in the change() method
 * Ex. add gamblingLuck as field:
 *  public float gamblingLuck;
 * Then in change() add: 
 *  gamblingLuck = statChangeFunc(gamblingLuck, other.gamblingLuck, "Gambling Luck");
 *  "Gambling Luck" will be the name used for the stat in game
 *  
 *  NOTE: only accepts stats that can be represented by floats
 *  Ex. [OK] int,float,double       [NOT] String, char, array[], Object 
 */
public class UpgradeableStatContainer: ScriptableObject
{
    //ADD NEW STAT FIELDS HERE
    public float health;
    public float stamina;
    public int ammo;
    public float damage;
    public float moveSpeed;
    public int numJumps;
    public float slidePower;
    public float dashPower;

    /*
     * HELPER FUNCTION for other methods: 
     * ADD NEW STATS HERE, KEEP STATS IN SAME ORDER AS ABOVE
     * 
     * Changes each stat in this container by some other UpgradeableStatContainer, given some function applied to each stat
     * statChangeFunc is a function take takes in two float values, the stat values, aswell as a string reprenting the stats in game name
     * statChange Func returns a float representing the new resulting stat value
     */
    void change(UpgradeableStatContainer other, Func<float, float, string,float> statChangeFunc)
    {
        health = statChangeFunc(health, other.health,"Health");
        stamina = statChangeFunc(stamina, other.stamina,"Stamina");
        ammo = (int)statChangeFunc(ammo, other.ammo,"Ammo");
        damage = statChangeFunc(damage, other.damage, "Damage");
        moveSpeed = statChangeFunc(moveSpeed, other.moveSpeed, "Move Speed");
        numJumps = (int)statChangeFunc(numJumps, other.numJumps, "Jumps");
        slidePower = statChangeFunc(slidePower, other.slidePower,"Slide Power");
        dashPower = statChangeFunc(dashPower, other.dashPower, "Dash Power");
    }

    /*
     * Adds the stats of the other UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void add(UpgradeableStatContainer other)
    {
        change(other, (a, b,_) => a + b);
        //(a, b,_) => a + b is a lambda statement that defines a function taking in two floats a and b and returning the float sum a+b
    }

    /*
     * Set the stats of the other UpgradeableStatContainer to this UpgradeableStatContainer's stats
     */
    public void set(UpgradeableStatContainer other)
    {
        change(other, (a, b,_) => b);
        //(a, b,_) => b is a lambda statement that defines a function taking in two floats a and b and returning float b
    }

    /*
     * Printout of all stats in this container, each line StatName: statValue 
     */

    public virtual string printStats()
    {
        List<string> lines = new List<string>();
        change(this, (value, _, name) => {
            if (value != 0)
            {
                lines.Add($"{name}: {value}");
            }
            return value;
        });
        return string.Join("\n", lines);


    }
    public virtual string printAllStats()
    {
        List<string> lines = new List<string>();
        change(this, (value,_,name) => {
            lines.Add($"{name}: {value}");
            return value;
        });
        return string.Join("\n", lines);
        

    }

}