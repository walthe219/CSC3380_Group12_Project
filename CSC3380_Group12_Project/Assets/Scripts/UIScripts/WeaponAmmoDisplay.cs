using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WeaponAmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoDisplay;
    [SerializeField] PlayerStats CurrentPlayerStats;
    [SerializeField] PlayerStats BasePlayerStats;
    public float lowAmmoIndicatorPrct = 0.1f;

    void Start()
    {
        if (CurrentPlayerStats == null)
        {
            Debug.Log("CurrentPlayerStats not assigned in insepctor");
        }

        if (ammoDisplay == null)
        {
            Debug.LogError("ammoDisplay Text UI is not assigned in Inspector!");
        }

        updateAmmo();
    }

    void updateAmmo()
    {
        //Always display current ammo with two digits
        ammoDisplay.text = CurrentPlayerStats.ammo.ToString("00");
        if(CurrentPlayerStats.ammo > BasePlayerStats.ammo)
        {
            ammoDisplay.color = Color.green;
        }
        else if(CurrentPlayerStats.ammo < BasePlayerStats.ammo * lowAmmoIndicatorPrct)
        {
            ammoDisplay.color = Color.red;
        }
        else
        {
            ammoDisplay.color = Color.white;
        }
    }

    void Update()
    {
        updateAmmo();
    }
}
