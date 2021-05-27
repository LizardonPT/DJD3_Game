using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private GameObject deathScreen = default;
    [SerializeField] private TextMeshPro floatingTextPrefab = default;
    private GameObject player;

    private TextMeshPro dmgText;
    void Awake()
    {
        // If its the player initializes hp interface
        if (gameObject.layer == 9) hpText.text = "Health: " + hp.ToString();
        // If not locates player Gameobject
        else player = GameObject.Find("Player");
    }
    void Start() {}
    public void HPModifier(int modHP, string damageType)
    {
        // Calculates the amount of lost hp and updates it
        int decrease = Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));
        hp -= decrease;
        
        // If its the player updates the interface
        if (gameObject.layer == 9)
        {
            hpText.text = "Health: " + hp.ToString();
        }
        // If not spawns the damage text on the enemy location
        else
        {
            // spawns the Text Object
            dmgText = Instantiate(floatingTextPrefab, transform.position,
                Quaternion.FromToRotation(transform.position, player.transform.position), transform);
            //Debug.Log(Quaternion.FromToRotation(transform.position, player.transform.position));
            
            // Updates the text according to the value the enemy lost
            dmgText.text = decrease.ToString();

            //Destroy the object after 2s
            Destroy(dmgText, 2f);
        }

        // Alway updates the kill counter for layer Dummy
        if(gameObject.tag == "Switch")
        {
            transform.Find("IndicatorOn").gameObject.SetActive(true);
            transform.Find("IndicatorOff").gameObject.SetActive(false);
            transform.Find("GameObject").Find("UnlockableDoor").gameObject.GetComponentInChildren<Animator>().SetBool("doorUnlock",true);
            Destroy(GetComponent<HP>());
            hp = 1;
        }

        // If the hp is less or equal to 0 it dies
        if(hp <= 0)
        {
            // Destroys the game object
            Destroy(gameObject);

            // If the target is the player
            if(gameObject.layer == 9)
            {
                // Spawns the Death Screen
                deathScreen.SetActive(true);
                // Stops game Time
                Time.timeScale = 0f;
                // Unlocks Mouse movement
                Cursor.lockState = CursorLockMode.None;
            }
            // If not the player...
            else
            {
                // ... Updates the player Kill Streak
                player.GetComponent<KillCounter>().KillUpdate();
            }
        }
    }

    // Kill streak reward hp regeneration
    public void RegenHP()
    {
        // If hp is less than max
        if(hp < maxHP)
        {
            // Regens 1 hp
            hp++;
            // Updates HP interface
            hpText.text = "Health: " + hp.ToString();
        }
    }
}
