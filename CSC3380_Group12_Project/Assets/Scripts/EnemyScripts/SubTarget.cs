using UnityEngine;
using System;

public class SubTarget : MonoBehaviour
{
    public Target target;
    public float dmgMult = 1f;
    public float limbHealth = 50f;
    public AudioClip hitSFX;

    public event Action OnDamageTaken;
    public event Action OnDestoryed;

    public void TakeDamage(DamageValue damage)
    {
        damage.addDmgMult(dmgMult);

        

        float limbDamage = damage.getFinalDmg();

        if (limbHealth <= 0f)
        {
            Debug.Log("The bitch is crippled!");
            Cripple(damage);
        }
        else
        {
            if (dmgMult > 1.0f)
                damage.isCriticalPoint = true;

            limbHealth -= limbDamage;
            target.TakeDamage(damage);

            OnDamageTaken?.Invoke();
        }

    }

    void Cripple(DamageValue damage)
    {
        damage.addDmgMult(0.5f);
        target.TakeDamage(damage);

        OnDamageTaken?.Invoke();
        OnDestoryed?.Invoke();
    }
}
