using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText = default;

    public int killScore = 0;
    private float timer = 0;
    private int confirmScore = 2;
    [SerializeField] private float maxTimer = default;

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            timer = 0;
            killScore = 0;
            confirmScore = 2;
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
        // Every 2 kills ...

        // Gain 1 HP
        if(killScore % 2 == 0 && killScore != 0 && killScore/confirmScore == 1)
        {
            gameObject.GetComponent<HP>().RegenHP();
            confirmScore += 2;
        }
        // Reduces Dash Cooldown
        gameObject.GetComponent<ThirdPersonMovement>().RapidCharge(Mathf.Max(1 - (killScore/2 * 0.1f),0.5f));
        // Augment Speed
        gameObject.GetComponent<ThirdPersonMovement>().speed = 6 + Mathf.Min(killScore/2,6);

        // Every 3 kills, augment energy gain
        gameObject.GetComponent<Energy>().AdrenalineBoost(Mathf.Min(killScore/3,4));
    }
}
