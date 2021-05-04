using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int electArmor = default;
    [SerializeField] private int plasmaArmor= default;
    public int ArmorReduction(string damageType)
    {
        if(damageType == "electric") return electArmor;
        else return plasmaArmor;
    }
}
