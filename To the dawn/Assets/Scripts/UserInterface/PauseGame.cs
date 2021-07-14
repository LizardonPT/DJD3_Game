using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI = default;
    [SerializeField] private GameObject settingsUI = default;
    [SerializeField] private AudioSource pauseSound = default;
    [SerializeField] private AudioSource soundTrack = default;
    private int weapon = default;


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
        if(GameObject.Find("Player").GetComponent<ElectricGun>().enabled)
        {
            weapon = 0;
            GameObject.Find("Player").GetComponent<ElectricGun>().enabled = false;
        }
        if(GameObject.Find("Player").GetComponent<PlasmaGun>().enabled)
        {
            weapon = 1;
            GameObject.Find("Player").GetComponent<PlasmaGun>().enabled = false;
        }
        soundTrack.Pause();
        pauseSound.Play();
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        if(weapon == 0)
        {
            GameObject.Find("Player").GetComponent<ElectricGun>().enabled = true;
        }
        if(weapon == 1)
        {
            GameObject.Find("Player").GetComponent<PlasmaGun>().enabled = true;
        }
        soundTrack.UnPause();
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
