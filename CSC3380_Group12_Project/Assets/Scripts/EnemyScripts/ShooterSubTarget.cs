using UnityEngine;

public class ShooterSubTarget : MonoBehaviour
{
    public ShooterTarget target;
    public float dmgMult = 1f;
    public float limbHealth = 50f;



    public void TakeDamage(float damage)
    {
        float totalDmg = damage * dmgMult;

        if (limbHealth <= 0f)
        {
            Debug.Log("The bitch is crippled!");
            Cripple(totalDmg);
        }
        else
        {
            limbHealth -= totalDmg;
            target.TakeDamage(totalDmg, transform.name);
        }

    }

    void Cripple(float damage)
    {
        target.TakeDamage(damage * 0.5f, transform.name);
    }
}
