using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class healthUIMngr : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats DefaultStats;

    private float currentHealth;
    private int maxHealth;
    private int healAmt;
    private int damage;
    

    public HealthBar healthBar;
    public TextMeshProUGUI HealthDisplay;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthDisplay = GameObject.Find("HealthDisplay").GetComponent<TextMeshProUGUI>();
        if(CurrentPlayerStats == null){
            Debug.Log("CurrentPlayerStats not in inspector");
        }
        
        //CurrentPlayerStats.health = DefaultStats.health;

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
            HealthDisplay.text = (Mathf.Ceil((int)CurrentPlayerStats.health)).ToString() + "/" + (Mathf.Ceil((int)DefaultStats.health)).ToString();
        }
        else{
            Debug.Log("Health is null");
        }
    }

/* correct format ************************************* make sure you drag the upgrade into inspector of the script that wants to add upgrade, follow this format, and then set a bool at the end
//so you can do if(bool){dash();} for example
    public void ApplyDashUpgrade(){
    if (dashUpgrade != null){
        // Ensure the upgrade knows which event to trigger
        dashUpgrade.unlocks = new UnlockFunctions.Unlockable[] { UnlockFunctions.Unlockable.DASH };

        Upgrade dash = new Upgrade(dashUpgrade);
        upgradeManager.addUpgrade(dash); // This calls applyUpgrade(), which calls activate(), triggering the event

        Debug.Log("Applied Dash!");
    }
}
*/

    // Update is called once per frame
    void Update()
    {   
        if(CurrentPlayerStats.health >= 0){
        healthtoText();
        }
       
    }
}

