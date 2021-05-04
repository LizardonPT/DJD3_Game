using UnityEngine;
using TMPro;

public class Energy : MonoBehaviour
{
    [SerializeField] private int energyMax = 100;
    public int energy;
    private float timer;
    private int boost;
    [SerializeField] private TextMeshProUGUI energyText;

    void Start()
    {
        energy = energyMax;
        energyText.text = energyMax.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(energy == energyMax)
        {
            timer = 0;
        }
        else if((energy < energyMax) && (timer >= 1f))
        {
            energy++;
            if(boost > 0)
            {
                energy += boost;
                if(energy > energyMax)
                {
                    energy = energyMax;
                }
            }
            timer = 0;
        }
        energyText.text = energy.ToString();
    }

    public void AdrenalineBoost(int extraBoost)
    {
        boost = extraBoost;
    }

    public void UpdateEnergy(int usedEnergy)
    {
        energy -= usedEnergy;
    }
}
