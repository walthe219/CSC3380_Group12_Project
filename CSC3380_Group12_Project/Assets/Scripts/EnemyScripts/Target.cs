  using System;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float totalHealth = 100f;
    public RunnerReferences runRef;

    public AudioClip hitSFX;

    public event Action OnDamageTaken;
    public event Action OnDeath;

    [SerializeField] GameObject damageNumberPrefab;

    public void TakeDamage(float damage, string location, Vector3 pos)
    {
        totalHealth -= damage;
        OnDamageTaken?.Invoke();

        spawnDamageNumber((int)damage, 0, pos);

        if (totalHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if(runRef != null)
            runRef.agent.isStopped = true;

        OnDeath?.Invoke();
        OnDeath = null;
        
        //float deathTimer = 0f;
        //runRef.anim.SetBool("isDead", true);
        /*if (deathTimer < 3f)
        {
            deathTimer += Time.deltaTime;
        }
        else
        {
            
            gameObject.SetActive(false);
        }*/
        gameObject.SetActive(false);

    }

   void spawnDamageNumber(int damageTaken, int numCrits, Vector3 pos)
    {
        if(damageNumberPrefab != null)
        {
            var dmgNum = Instantiate(damageNumberPrefab, pos, Quaternion.identity).GetComponent<DamageNumberScript>();
            dmgNum.Initialize(damageTaken, numCrits);
        }
    }
}
