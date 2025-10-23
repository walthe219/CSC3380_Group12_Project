using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] PlayerStats currentStats;
    public Slider slider;

    public void setMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health){
            slider.value = health;
    }
   
}
