using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private GameObject deathScreen = default;
    void Awake()
    {
        if(gameObject.layer == 9) hpText.text =  "Health: " + hp.ToString();
    }

    public void HPModifier(int modHP, string damageType)
    {
        hp -= Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));

        if(gameObject.layer == 9)
        {
            hpText.text = "Health: " + hp.ToString();
        }

        if(gameObject.layer == 10)
        {
            GameObject.Find("Player").GetComponent<KillCounter>().
                KillUpdate();
        }

        if(hp <= 0)
        {
            Destroy(gameObject);

            if(gameObject.layer == 9)
            {
                deathScreen.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                GameObject.Find("Player").GetComponent<KillCounter>().
                    KillUpdate();
            }
        }
    }

    public void RegenHP()
    {
        if(hp < maxHP)
        {
            hp++;
            hpText.text = "Health: " + hp.ToString();
        }
    }
}
