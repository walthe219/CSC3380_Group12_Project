using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// This class contains all the parameters to describe damage values
// Many values affect final damage like base damage, random crit, headshot multiplier, damage resistance
// all affect end result of damage numbers
// build up over many sources, gun, sub target, target
// An instance of a DamageValue is created at the soucre and passed to each script that affects or use that damage

public class DamageValue
{
    public GameObject origin;
    public Vector3 originPos;
    public Vector3 hitPos;

    float baseDamage = 0;
    float damageMultipliers = 1;
    float criticalChance = 0;
    float criticalMultipliers = 0;

    public bool isCriticalPoint;

    int numCrits = -1;

    public DamageValue(float baseDMG, GameObject ori, Vector3 hit)
    {
        baseDamage = baseDMG;
        origin = ori;
        originPos = ori.transform.position;
        hitPos = hit;
    }

    public void addDmgMult(float multiplier)
    {
        damageMultipliers *= multiplier;
    }

    public void addCritMult(float critMultiplier)
    {
        criticalMultipliers += critMultiplier;
    }

    public void addCritChance(float critChance)
    {
        criticalChance += critChance;
    }

    public float getFinalDmg()
    {
        Debug.Log($"Base: {baseDamage}, Mult: {damageMultipliers}, CritMul: {criticalMultipliers}, CritChance {criticalChance}, NumCrits: {numCrits}");
        return baseDamage * getTotalDmgMult();
    }

    public float getTotalDmgMult()
    {
        return getTotalCritMult() + damageMultipliers;
    }

    public float getTotalCritMult()
    {
        return getNumCrits() * criticalMultipliers;
    }

    public int getNumCrits()
    {
        // If has already been calculated
        if(numCrits > -1)
            return numCrits;

        numCrits = (int)(criticalChance);
        if (UnityEngine.Random.value < criticalChance % 1)
            numCrits++;

        return numCrits;
    }



}
