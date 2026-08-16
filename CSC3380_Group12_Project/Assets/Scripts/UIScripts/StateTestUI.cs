using UnityEngine;
using TMPro;

public class StateTestUI : MonoBehaviour
{
    [SerializeField] DasherBehavior dasher;
    [SerializeField] TMP_Text tmp;

    // Update is called once per frame
    private void Start()
    {
        dasher.OnStateChange += updateText;
    }

    void updateText(string state)
    {
        tmp.text = state;
    }
}
