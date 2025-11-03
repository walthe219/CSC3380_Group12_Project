using TMPro;
using UnityEngine;

//Script for TextScreen prefab, used to preview room reward
public class TextDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string prefix = "Reward: ";


    //could maybe use an event to call this instead?
    public void changeText(string s)
    {
        text.text = prefix + s;
    }
}
