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
        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not in inspector");
        }
        
        CurrentPlayerStats.health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

      void takeDmg(int damage){ //test func
        if(CurrentPlayerStats.health > 0){
        CurrentPlayerStats.health -= damage;
        }
        healthBar.setHealth(CurrentPlayerStats.health); 
    }

    void heal(int healAmt){ //test func
        if(CurrentPlayerStats.health < 100){
        this.healAmt = healAmt;
        CurrentPlayerStats.health = CurrentPlayerStats.health + healAmt;
        }
        healthBar.setHealth(CurrentPlayerStats.health);
    }


    void healthtoText(){
         if(HealthDisplay != null){
            HealthDisplay.text = CurrentPlayerStats.health.ToString();
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

