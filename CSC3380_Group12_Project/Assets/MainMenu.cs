using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

public GameObject MainMenuUI;

    public void PlayGame(){
        //MainMenuUI.SetActive(false);
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);

        //SceneManager.LoadScene("UI_Scene", LoadSceneMode.Additive);
        

    }

    public void QuitGame(){
        Debug.Log("Quitting Game");
        Application.Quit();
    }
    
}
