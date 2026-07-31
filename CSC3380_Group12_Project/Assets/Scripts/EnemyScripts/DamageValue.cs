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
    public float baseDamage;
    public List<float> damageMultipliers;
    public float criticalChance;
    public List<float> criticalMultipliers;

    public DamageValue(int baseDMG)
    {
        baseDamage = baseDMG;
    }
    public void applyDamageMultiplier(float multiplier)
    {
        damageMultipliers.Add(multiplier);
    }

    public void applyDamageMultiplier(List<float> multipliers)
    {
        damageMultipliers.AddRange(multipliers);
    }

    public void applycriticalMultiplier(float critMultiplier)
    {
        criticalMultipliers.Add(critMultiplier);
    }

    public void applycriticalMultiplier(List<float> critMultipliers)
    {
        criticalMultipliers.AddRange(critMultipliers);
    }

    public float calculateFinalDmg()
    {
        return baseDamage * calculateFinalDamagetMultiplier();
    }

    public float calculateFinalDamagetMultiplier()
    {
        return calculateFinalCritMultiplier() + damageMultipliers.Sum();
    }

    public float calculateFinalCritMultiplier()
    {
        return calculateNumCrits() * criticalMultipliers.Sum();
    }

    public float calculateNumCrits()
    {
        int numCrits = (int)(criticalChance);
        if (UnityEngine.Random.value < criticalChance % 1)
            numCrits++;

        return numCrits;
    }

}
