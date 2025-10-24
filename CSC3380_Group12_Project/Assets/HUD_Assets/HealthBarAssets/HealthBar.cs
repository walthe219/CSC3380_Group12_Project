using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] PlayerStats CurrentPlayerStats;
    public Slider slider;

    
    private void Start()
    {
        if (CurrentPlayerStats != null)
        {
            // Initialize the slider using the player’s current health
            setMaxHealth(CurrentPlayerStats.health);
        }
        else
        {
            Debug.LogError("CurrentPlayerStats is not assigned in the Inspector!");
        }
    }

    public void setMaxHealth(float health){
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(float health){
            slider.value = health;
    }
   
}
