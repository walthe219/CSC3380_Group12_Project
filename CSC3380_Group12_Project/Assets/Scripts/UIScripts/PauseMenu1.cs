using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsmenu;
    public GameObject UIContainer;
    public GameObject HSMenu;
    public GameOverScreen gameoverscreen;

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

public void Resume(){
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
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
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
    Cursor.visible = true;             // Show cursor to interact with buttons
    Cursor.lockState = CursorLockMode.None;

    //Hide UI when opening pause menu
    UIContainer.SetActive(false);

    HSMenu.SetActive(false);

    }

    //load menu function is just a test function from tutorial
    public void LoadMenu(){
    Debug.Log("Loading Menu");
    }

    public void QuitGame(){
        Debug.Log("Quiting Game");
        SceneManager.LoadScene("Menu1", LoadSceneMode.Single);
    }

    public void LoadUpgradeMenu(){
        Debug.Log("Loading Upgrade Menu");
    }

}
