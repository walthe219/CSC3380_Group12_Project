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

    public void TakeDamage(float baseDamage,Vector3 pos)
    {
        float currTotalMult = dmgMult;
        float limbDamage = baseDamage * currTotalMult;

        if (limbHealth <= 0f)
        {
            Debug.Log("The bitch is crippled!");
            Cripple(currTotalMult);
        }
        else
        {
            limbHealth -= currTotalMult;
            target.TakeDamage(baseDamage * currTotalMult, transform.name, pos);
            OnDamageTaken?.Invoke();
        }

    }

    void Cripple(float damage)
    {   
        target.TakeDamage(damage * 0.5f, transform.name, transform.position);
        OnDamageTaken?.Invoke();
        OnDestoryed?.Invoke();
    }
}
