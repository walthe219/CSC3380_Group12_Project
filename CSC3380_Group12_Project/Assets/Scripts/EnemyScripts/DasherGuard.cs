using UnityEngine;

public class DasherGuard : MonoBehaviour
{
    [SerializeField] DasherReference dasher;
    [SerializeField] Collider collider;

    private void Start()
    {
        dasher.behavior.OnStateChange += checkGuard;

        if(!collider)
            collider = GetComponent<Collider>();

        DeactivateGuard();
    }

    void checkGuard(string state)
    {
        if (state == "GUARD")
        {
            ActivateGuard();
        }
        else
        {
            DeactivateGuard();
        }
    }

    void ActivateGuard()
    {
        collider.enabled = true;
        dasher.guard.enabled = true;
    }

    void DeactivateGuard()
    {
        collider.enabled = false;
        dasher.guard.enabled = false;
    }
}
