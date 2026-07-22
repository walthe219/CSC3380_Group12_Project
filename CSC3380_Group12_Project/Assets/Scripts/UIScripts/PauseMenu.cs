using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public InputActionAsset InputAsset;
    public GameObject pauseMenuUI;
    public GameObject optionsmenu;
    public GameObject UIContainer;
    public GameObject HSMenu;
    public GameOverScreen gameoverscreen;

    public static event Action OnPause;
    public static event Action OnResume;

    private InputActionMap UIMap;
    private InputActionMap PlayerMap;

    private void Awake()
    {
        UIMap = InputAsset.FindActionMap("UI");
        PlayerMap = InputAsset.FindActionMap("Player");
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape)){
            if(!GameIsPaused && !gameoverscreen.gosActive){
                Pause();
            }
            else{
                Resume();
            }
       }
    }

    void Resume(){
        OnResume?.Invoke();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerMap?.Enable();

        Cursor.visible = false;            // Hide cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;

        //Make sure options menu is closed when pressing escape
        if(optionsmenu.activeSelf){
            optionsmenu.SetActive(false);
        }

        //redisplay UI when pause menu is closed
        UIContainer.SetActive(true);
    }

    void Pause(){
        OnPause?.Invoke();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayerMap?.Disable();

        Cursor.visible = true;             // Show cursor to interact with buttons
        Cursor.lockState = CursorLockMode.None;

        //Hide UI when opening pause menu
        UIContainer.SetActive(false);

        HSMenu.SetActive(false);
    }

    void QuitGame(){
        Debug.Log("Quiting Game");
        SceneManager.LoadScene("Menu1", LoadSceneMode.Single);
    }
}
