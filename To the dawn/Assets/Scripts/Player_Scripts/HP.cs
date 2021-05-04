using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private TextMeshProUGUI hpText = default;
    void Awake()
    {
        if(gameObject.layer == 9) hpText.text = hp.ToString();
    }

    public void HPModifier(int modHP, string damageType)
    {
        hp -= Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));

        if(gameObject.layer == 9)
        {
            hpText.text = hp.ToString();
        }

        if(hp <= 0)
        {
            Destroy(gameObject);

            if(gameObject.layer == 9)
            {
                //DeathScreen
            }
            else
            {
                GameObject.Find("Player").GetComponent<KillCounter>().
                    KillUpdate();
            }
        }
    }
}
