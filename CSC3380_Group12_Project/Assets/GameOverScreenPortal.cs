using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScreenPortal : MonoBehaviour
{
    [SerializeField] PlayerStats CurrentPlayerStats;
    public GameObject gameoverscreenportal;

    public void SetupP(){
        gameoverscreenportal.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;             // Show cursor to interact with buttons
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitToMainMenuP(){
        SceneManager.LoadScene("Menu1", LoadSceneMode.Single);
    }

    public void QuitToDesktopP(){
        Application.Quit();
    }

    public void RestartGameP(){
        SceneManager.LoadScene("TestSceneWithUI&Menu", LoadSceneMode.Single);
    }

    void Update(){
        // Get all portalScript instances in the scene
        portalScript[] portals = FindObjectsOfType<portalScript>();

    
    foreach (portalScript portal in portals){
        if (portal.portalHealth <= 0){
            SetupP();  // trigger game over
            break;     // no need to check the rest
            }
        }
    }
}
