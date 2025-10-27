using UnityEngine;

public class Target : MonoBehaviour
{
    public float totalHealth = 100f;

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

        Destroy(gameObject);

    }
}
