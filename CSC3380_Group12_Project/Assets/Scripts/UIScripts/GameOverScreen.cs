using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;
    public GameObject gameoverscreen;
    public GameObject UIContainer;
    public bool gosActive;

    public void Setup(){
        gameoverscreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;             // Show cursor to interact with buttons
        Cursor.lockState = CursorLockMode.None;
        gosActive = true;
        if (UIContainer != null)
        {
            UIContainer.SetActive(false);
        }
    }
    private void Start()
    {
        gosActive = false; 
    }
    public void QuitToMainMenu(){
        SceneManager.LoadScene("Menu1", LoadSceneMode.Single);
    }

    public void QuitToDesktop(){
        Application.Quit();
    }

    public void RestartGame(){
        SceneManager.LoadScene("TestSceneWithUI&Menu", LoadSceneMode.Single);
    }

    void Update(){
        if(CurrentPlayerStats.health <= 0){
            Setup();
        }
    }
}
