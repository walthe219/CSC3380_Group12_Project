using UnityEngine;

public class ToggleScripts : MonoBehaviour
{
    public GameObject CrossCrosshair;
    public GameObject SquareCrossHair;

    public void CrosshairToggle(bool cht)
    {
        if (cht == true)
        {
            CrossCrosshair.SetActive(false);
            SquareCrossHair.SetActive(true);
            cht = false;
        }
        else {
            CrossCrosshair.SetActive(true);
            SquareCrossHair.SetActive(false);
            cht = true;
        }
    }

}
