using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;

    public void HPModifier(int modHP, string damageType)
    {
        hp -= Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));

        if(gameObject.layer == 9)
        {
            // Ui code
        }

        if(hp < 0)
        {
            hp = 0;
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<KillCounter>().killUpdate();


        }
    }
}
