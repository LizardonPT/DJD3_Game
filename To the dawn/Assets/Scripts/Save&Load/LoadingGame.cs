using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGame : MonoBehaviour
{
    public bool loadIsOn;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SeeLoad()
    {
        loadIsOn = true;
        GameObject.Find("MCTryout").GetComponent<Animator>().SetBool("Death", true);
        GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", true);
    }

}
