using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText = default;
    [SerializeField] private float maxTimer = default;

    public int killScore = 0;
    private float timer = 0;
    private int confirmScore = 2;

    // Update is called once per frame
    void Update()
    {
        // Manages the timer
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

        // Initializes the KillScore interface
        if(killScore == 0)
        {
            killText.text = "";
        }
        // Updates the KillScore interface
        else
        {
            killText.text = "Kills: " + killScore.ToString();
        }
    }

    public void KillUpdate()
    {
        killScore++;
        // Resets the timer
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
        // Augment energy gain
        gameObject.GetComponent<Energy>().AdrenalineBoost(Mathf.Min(killScore/2,4));
    }
}
