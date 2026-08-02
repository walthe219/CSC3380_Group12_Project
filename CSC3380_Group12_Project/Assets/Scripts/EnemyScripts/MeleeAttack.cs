using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damage = 10;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            var playerHealth = other.GetComponentInParent<PlayerDamageManager>();
            playerHealth.dealDamage(damage);
            
        }
        else if (other.gameObject.tag == "isPortal")
        {
            portalScript portal = other.gameObject.GetComponent<portalScript>();
            portal.portalHealth -= damage;
        }
    }
}
