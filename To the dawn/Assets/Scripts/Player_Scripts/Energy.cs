using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private int energyMax = 100;
    public int energy;
    private float timer;

    void Start()
    {
        energy = energyMax;
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
            Debug.Log(energy);
            timer = 0;
        }
    }

    public void UpdateEnergy(int usedEnergy)
    {
        energy -= usedEnergy;
    }
}
