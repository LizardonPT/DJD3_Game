using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI = default;
    [SerializeField] private GameObject settingsUI = default;

    private void Update()
    {
        // Manages the paused game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if ((!gameIsPaused) && (Time.timeScale != 0f))
            {
                Pause();
            }
            else if (gameIsPaused && settingsUI.activeSelf)
            {
                settingsUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else
            {
                Resume();
            }
        }
    }
    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
