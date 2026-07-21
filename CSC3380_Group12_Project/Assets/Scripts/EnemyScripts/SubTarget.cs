using UnityEngine;

public class SubTarget : MonoBehaviour
{
    public Target target;
    public float dmgMult = 1f;
    public float limbHealth = 50f;
    public AudioClip hitSFX;



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
            target.TakeDamage(baseDamage, transform.name, pos);
        }

    }

    void Cripple(float damage)
    {
        target.TakeDamage(damage * 0.5f, transform.name, transform.position);
    }
}
