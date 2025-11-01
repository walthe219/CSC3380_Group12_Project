using TMPro;
using UnityEngine;


public class MouseSensDisplay : MonoBehaviour
{
    public TextMeshProUGUI sensDisplay;
    private float sens = 120f;

    // Update is called once per frame
    void Update()
    {
        sensDisplay = GetComponent<TextMeshProUGUI>();
        sensDisplay.text = ((int)sens).ToString();
    }

    public void SetSensitivity(float newSens)
    {
        sens = newSens;
    }
}
