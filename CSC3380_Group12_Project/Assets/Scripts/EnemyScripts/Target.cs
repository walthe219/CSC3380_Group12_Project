using System;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float totalHealth = 100f;
    public RunnerReferences runRef;
    public event Action OnDeath;
    public void TakeDamage(float damage, string location)
    {

        totalHealth -= damage;

        if (totalHealth <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        OnDeath?.Invoke();
        OnDeath = null;
        Destroy(gameObject);

    }
}
