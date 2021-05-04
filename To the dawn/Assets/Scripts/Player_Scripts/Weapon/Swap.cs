using UnityEngine;
using TMPro;

public class Swap : MonoBehaviour
{
    [SerializeField] private float swapCooldown = 2f;

    [SerializeField] private PlasmaGun plGun;
    [SerializeField] private ElectricGun elGun;
    [SerializeField] private int energySwap = 25;

    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private Animator interfaceAnim;
    private float timer;

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
        if(gameObject.GetComponent<Energy>().energy - energySwap > 0)
        {
            gameObject.GetComponent<Energy>().UpdateEnergy(energySwap);
            if(plGun.enabled)
            {
                plGun.enabled = false;
                elGun.enabled = true;
                weaponText.text = "EletricGun";
            }
            else
            {
                plGun.enabled = true;
                elGun.enabled = false;
                weaponText.text = "PlasmaGun";
            }
        }
        else
        {
            interfaceAnim.SetTrigger("NoEnergy");
        }
    }
}
