using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText;

    public int killScore = 0;

    private float timer = 0;
    [SerializeField] private float maxTimer;

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            timer = 0;
            killScore = 0;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        killText.text = killScore.ToString();
    }

    public void killUpdate()
    {
        killScore += 1;
        timer = maxTimer;
    }
}
