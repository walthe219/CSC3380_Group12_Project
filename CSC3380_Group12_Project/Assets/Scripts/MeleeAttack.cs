using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            GameObject playerHealth = GameObject.FindWithTag("HealthBar");
            if (playerHealth != null)
            {
                HealthBar hb = playerHealth.GetComponent<HealthBar>();
                hb.CurrentPlayerStats.health -= 5;
                hb.setHealth(hb.CurrentPlayerStats.health);
            }
        }
        else if (other.gameObject.tag == "isPortal")
        {
            portalScript portal = other.gameObject.GetComponent<portalScript>();
            portal.portalHealth -= 5;
        }
    }

    void Update()
    {
        
    }
}
