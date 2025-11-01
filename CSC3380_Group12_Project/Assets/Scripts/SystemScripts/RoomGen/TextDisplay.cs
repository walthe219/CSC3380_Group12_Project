using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string prefix = "Reward: ";

    public void changeText(string s)
    {
        text.text = prefix + s;
    }
}
