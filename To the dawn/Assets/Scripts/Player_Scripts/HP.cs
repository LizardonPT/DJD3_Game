using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private GameObject deathScreen = default;
    [SerializeField] private TextMeshPro FloatingTextPrefab = default;
    private GameObject player;

    private TextMeshPro dmgText;
    void Awake()
    {
        if (gameObject.layer == 9) hpText.text = "Health: " + hp.ToString();
        else player = GameObject.Find("Player");
    }

    public void HPModifier(int modHP, string damageType)
    {
        int decrease = Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));
        hp -= decrease;
        if (gameObject.layer == 9)
        {
            hpText.text = "Health: " + hp.ToString();
        }
        else
        {
            dmgText = Instantiate(FloatingTextPrefab, transform.position,
                Quaternion.FromToRotation(transform.position, player.transform.position), transform);
            //Debug.Log(Quaternion.FromToRotation(transform.position, player.transform.position));
            dmgText.text = decrease.ToString();
            Destroy(dmgText, 2f);
        }

        if(gameObject.layer == 10)
        {
            player.GetComponent<KillCounter>().KillUpdate();
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
                player.GetComponent<KillCounter>().KillUpdate();
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
