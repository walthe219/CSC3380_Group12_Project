using UnityEngine;

public class DasherAttack : MonoBehaviour
{
    [SerializeField] Collider collider;
    [SerializeField] float dashDamage;

    private void Start()
    {
        collider = GetComponent<Collider>();
        ToggleOFF();
    }

    public void ToggleOn()
    {
        collider.enabled = true;
    }

    public void ToggleOFF()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " hit by dasher");
        var player = other.GetComponentInParent<PlayerDamageManager>();
        player.dealDamage(dashDamage);
    }
}
