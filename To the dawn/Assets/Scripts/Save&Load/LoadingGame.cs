using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGame : MonoBehaviour
{
    public bool loadIsOn;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SeeLoad()
    {
        loadIsOn = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
