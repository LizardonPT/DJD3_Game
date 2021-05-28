using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class which interact with the buttons pressed by the player in
/// the main menu.
/// </summary>
public class MenuButtons_Script : MonoBehaviour
{
    /// <summary>
    /// Public method that loads the next scene in the build settings order.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGround()
    {
        SceneManager.LoadScene("PlayGround Joao");
    }

    /// <summary>
    /// Public method that leaves the application, does not work on the editor.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
