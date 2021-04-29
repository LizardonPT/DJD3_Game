using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Energy : MonoBehaviour
{
    [SerializeField] private int energyMax = 100;
    public int energy;
    private float timer;

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
            timer = 0;
        }
        energyText.text = energy.ToString();
    }

    public void UpdateEnergy(int usedEnergy)
    {
        energy -= usedEnergy;
    }
}
