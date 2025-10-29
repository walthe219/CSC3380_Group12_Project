using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats DefaultStats;

    public float currentHealth;
    public int maxHealth;
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
        
        CurrentPlayerStats.health = DefaultStats.health;

        //CurrentPlayerStats.health = maxHealth; IMPORTANT: do not assign currentplayerstats.blah to a variable and then use the variable it does not work as expected
        healthBar.setMaxHealth(CurrentPlayerStats.health);
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

