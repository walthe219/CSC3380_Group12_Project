using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth = 100;

    public HealthBar healthBar;

 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

      void takeDmg(int damage){
        currentHealth -= damage;

        healthBar.setHealth(currentHealth); 
    }


    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.L)){
        takeDmg(10);
       }
    }
}
