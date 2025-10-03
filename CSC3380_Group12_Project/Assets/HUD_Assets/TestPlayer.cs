using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TestPlayer : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth = 100;
    private int sacAmt;

    public HealthBar healthBar;
    public AmmoDisplay AD;
    public TextMeshProUGUI HealthDisplay;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthDisplay = GameObject.Find("HealthDisplay").GetComponent<TextMeshProUGUI>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

      void takeDmg(int damage){
        if(currentHealth > 0){
        currentHealth -= damage;
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
    }
}

