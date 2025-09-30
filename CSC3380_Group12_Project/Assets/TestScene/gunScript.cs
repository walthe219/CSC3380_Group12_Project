using UnityEngine;
using UnityEngine.InputSystem;

public class gunScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject firePoint;
    public float fireRate;

    private float fireTimer = 0;

    InputAction shoot;

    private void Start()
    {
        shoot = InputSystem.actions.FindAction("Shoot");
    }

    private void Update()
    {
        if (shoot.IsPressed() && fireTimer ==0)
        {
            Instantiate(projectile,firePoint.transform.position, firePoint.transform.rotation);
            Debug.Log("Gun fired");
            fireTimer = 3;
        }

        if(fireTimer >= 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else
        {
            fireTimer = 0;
        }
    }


}
