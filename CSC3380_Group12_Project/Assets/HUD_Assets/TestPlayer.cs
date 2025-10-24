using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;

    public float currentHealth;
    public int maxHealth = 100;
    private int sacAmt;
    private int healAmt;

    public HealthBar healthBar;
    public AmmoDisplay AD;
    public TextMeshProUGUI HealthDisplay;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthDisplay = GameObject.Find("HealthDisplay").GetComponent<TextMeshProUGUI>();
        if(CurrentPlayerStats != null){
            currentHealth = CurrentPlayerStats.health;
        }
        else{
            Debug.Log("CurrentPlayerStats not in inspector");
        }
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

      void takeDmg(int damage){ //test func
        if(currentHealth > 0){
        currentHealth -= damage;
        }
        healthBar.setHealth(currentHealth); 
    }

    void heal(int healAmt){ //test func
        if(currentHealth < 100){
        this.healAmt = healAmt;
        currentHealth = currentHealth + healAmt;
        }
        healthBar.setHealth(currentHealth);
    }


    void healthtoText(){
         if(HealthDisplay != null){
            HealthDisplay.text = currentHealth.ToString();
        }
        else{
            Debug.Log("Health is null");
        }
    }

    void sacrifice(int sacAmt){ //sacAmt will allow us to pass in a value to divide health by
        if(Input.GetKeyDown(KeyCode.O)){
            currentHealth = currentHealth/sacAmt;
            //extra dmg
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthtoText();
        if(Input.GetKeyDown(KeyCode.L)){
        takeDmg(10);
       }
       if(Input.GetKeyDown(KeyCode.H)){
        heal(10);
       }
    }
}

