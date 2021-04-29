using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    [SerializeField] private float swapCooldown = 2f;

    [SerializeField] private PlasmaGun plGun;
    [SerializeField] private ElectricGun elGun;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer>= swapCooldown)
        {
            if(Input.GetButton("Swap"))
            {
                timer = 0f;
                SwapGun();
            }
        }
    }

    private void SwapGun()
    {
        if(plGun.enabled)
        {
            plGun.enabled = false;
            elGun.enabled = true;
        }
        else
        {
            plGun.enabled = true;
            elGun.enabled = false;
        }
    }
}
