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
        GameObject.Find("MCTryout").GetComponent<Animator>().SetBool("Death", true);
        GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", true);
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
