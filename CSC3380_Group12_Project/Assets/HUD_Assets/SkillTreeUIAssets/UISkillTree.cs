using UnityEngine;
using CodeMonkey.Utils;

public class UISkillTree : MonoBehaviour
{
  private void Awake() {
    transform.Find("Btn1").GetComponent<Button_UI>().ClickFunc = () => {
        Debug.Log("clicked");
    };
  }
}
