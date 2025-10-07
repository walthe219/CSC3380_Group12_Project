using UnityEngine;

public class UpgradeData : MonoBehaviour
{
    public AmmoDisplay ammodisplay;
    public HealthBar healthbar;
    public StaminaDisplay staminadisplay;
    public TestPlayer tp;


    private int health;
    private int ammo;
    private int stamina;







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = tp.currentHealth;
        ammo = ammodisplay.ammo;
        stamina = staminadisplay.stamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
