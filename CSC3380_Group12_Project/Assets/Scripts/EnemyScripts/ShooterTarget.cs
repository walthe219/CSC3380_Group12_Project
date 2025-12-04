using System;
using UnityEngine;

public class ShooterTarget : MonoBehaviour
{

    public float totalHealth = 100f;
    public ShooterReferences runRef;

    public event Action OnDeath;
    public void TakeDamage(float damage, string location)
    {

        totalHealth -= damage;

        if (totalHealth <= 0f)
        {
            Die();
        }

    }
    void OnEnable()
    {
        totalHealth = 100f;
    }

    void Die()
    {
        runRef.agent.isStopped = true;
        OnDeath?.Invoke();
        OnDeath = null;

        float deathTimer = 0f;
        runRef.anim.SetBool("isDead", true);
        if (deathTimer < 3f)
        {
            deathTimer += Time.deltaTime;
        }
        else
        {

            gameObject.SetActive(false);
        }


    }
}
