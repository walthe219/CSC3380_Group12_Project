using TMPro;
using UnityEngine;

//Script for TextScreen prefab, used to preview room reward
public class TextDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text description;
    [SerializeField] string prefix = "Reward: ";


    //could maybe use an event to call this instead?
    public void changeText(string s, string d)
    {
        text.text = prefix + s;
        description.text = d;
    }
}
