using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText = default;

    public int killScore = 0;

    private float timer = 0;
    [SerializeField] private float maxTimer = default;

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

        KillRewards();

        killText.text = "Kills: " + killScore.ToString();
    }

    public void KillUpdate()
    {
        killScore++;
        timer = maxTimer;
    }

    private void KillRewards()
    {
        gameObject.GetComponent<ThirdPersonMovement>().speed = 6 + Mathf.Min(killScore/2,6);
        gameObject.GetComponent<Energy>().AdrenalineBoost(Mathf.Min(killScore/3,4));
    }
}
