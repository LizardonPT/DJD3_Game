using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int electArmor;
    [SerializeField] private int plasmaArmor;
    public int ArmorReduction(string damageType)
    {
        if(damageType == "electric") return electArmor;
        else return plasmaArmor;
    }
}
