using UnityEngine;
using TMPro;

public class ToggleScripts : MonoBehaviour
{
    public GameObject CrossCrosshair;
    public GameObject SquareCrossHair;

    public void CrosshairToggle(int val)
    {
        if (val == 0) { 
            CrossCrosshair.SetActive(false);
            SquareCrossHair.SetActive(true);
        }
        if (val == 1)
        {
            CrossCrosshair.SetActive(true);
            SquareCrossHair.SetActive(false);
        }
    }

}
