using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayAndLoad : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
