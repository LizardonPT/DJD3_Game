using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] private int energyMax = 100;
    [SerializeField] private TextMeshProUGUI energyText = default;
    [SerializeField] private Image mask = default;
    public int energy;
    private float timer;
    private int boost;

    void Start()
    {
        // Initializes the interface and energy value
        energy = energyMax;
        energyText.text = "Energy: " + energyMax.ToString();
    }

    void Update()
    {
        // Updates the regeneration cooldown
        timer += Time.deltaTime;

        // Resets the cooldown when at max energy
        if(energy == energyMax)
        {
            timer = 0;
        }
        // If the energy is not Max and the cooldown is over...
        else if((energy < energyMax) && (timer >= 1f))
        {
            // ... Updates the enery
            energy++;
            // If the player has KillStreak gives him the bonuses
            if(boost > 0)
            {
                energy += boost;
                // If the bonus surpasses max energy resets energy to Max
                if(energy > energyMax)
                {
                    energy = energyMax;
                }
            }
            // Restarts the cooldown
            timer = 0;
        }
        // Update the energy interface
        energyText.text = "Energy: " + energy.ToString();

        GetCurrentFill();
    }

    public void AdrenalineBoost(int extraBoost)
    {
        // Defines the bonus from the kill Streak
        boost = extraBoost;
    }

    public void UpdateEnergy(int usedEnergy)
    {
        // Updates energy according to the expenditure
        energy -= usedEnergy;
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)energy / (float)energyMax;
        mask.fillAmount = fillAmount;
    }
}
