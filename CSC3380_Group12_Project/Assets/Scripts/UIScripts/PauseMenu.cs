using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu1 : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
       }
    }

public void Resume(){
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
    Cursor.visible = false;            // Hide cursor during gameplay
    Cursor.lockState = CursorLockMode.Locked;
}

void Pause(){
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
    Cursor.visible = true;             // Show cursor to interact with buttons
    Cursor.lockState = CursorLockMode.None;
}

public void LoadMenu(){
    Time.timeScale = 1f;
    SceneManager.LoadScene("Menu1");
}

public void QuitGame(){
    Debug.Log("Quiting Game");
    Application.Quit();
}

}
