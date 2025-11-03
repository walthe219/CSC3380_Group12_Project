using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

public GameObject MainMenuUI;

    public void PlayGame(){
        //MainMenuUI.SetActive(false); < ignore
        SceneManager.LoadScene("TestSceneWithUI&Menu", LoadSceneMode.Single);

        //SceneManager.LoadScene("UI_Scene", LoadSceneMode.Additive); < ignore
        

    }

    public void QuitGame(){
        //Debug.Log("Quitting Game");
        Application.Quit();
    }
    
}
