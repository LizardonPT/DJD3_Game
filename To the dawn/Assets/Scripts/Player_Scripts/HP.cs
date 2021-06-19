using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private Image mask = default;
    [SerializeField] private GameObject deathScreen = default;
    [SerializeField] private TextMeshPro dmgTextPrefab = default;
    [SerializeField] private TextMeshPro invulnerablePrefab = default;
    private GameObject player;
    private Animator anim;

    private TextMeshPro floatingText;
    void Awake()
    {
        // If its the player initializes hp interface
        if (gameObject.layer == 9)
        {
            hpText.text = "Health: " + hp.ToString();
        }
        // If not locates player Gameobject
        else
        {
            player = GameObject.Find("Player");
        }
        // Gets the FIRST animator it finds in the game object or children
        anim = GetComponentInChildren<Animator>();
    }
    // Do not remove its useful to destroy objects or components - this case is used.
    void Start() {}

    public void HPModifier(int modHP, string damageType)
    {
        // Calculates the amount of lost hp and updates it
        int decrease = Mathf.Max(0, modHP - gameObject.GetComponent<Armor>().ArmorReduction(damageType));
        hp -= decrease;

        // If its the player 
        if (gameObject.layer == 9)
        { // Updates the interface
            hpText.text = "Health: " + hp.ToString();
            GetCurrentFill();
        }
        // If not a pop up appears on the target location
        else
        {
            // If the target is a switch
            if(gameObject.tag == "Switch")
            {
                // Unlockes the door
                transform.Find("IndicatorOn").gameObject.SetActive(true);
                transform.Find("IndicatorOff").gameObject.SetActive(false);
                transform.Find("GameObject").Find("UnlockableDoor").gameObject.GetComponentInChildren<Animator>().SetBool("doorUnlock", true);
                //Destroys the object
                Destroy(GetComponent<HP>());
                hp = 1;
                // Placeholder visual feedback
                floatingText = Instantiate(dmgTextPrefab, transform.position,
                    Quaternion.FromToRotation(transform.position, player.transform.position), transform);
            }
            // If it is damaged
            else if (decrease > 0)
            {// Spawns the Text Object
                floatingText = Instantiate(dmgTextPrefab, transform.position,
                    Quaternion.FromToRotation(transform.position, player.transform.position), transform);

                // Updates the text according to the value the enemy lost
                floatingText.text = decrease.ToString();
            }
            // If it is not damaged
            else
            { // Spawns the invunerable Text Object
                floatingText = Instantiate(invulnerablePrefab, transform.position,
                    Quaternion.FromToRotation(transform.position, player.transform.position), transform);
            }
            
            //Destroy the object after 2s
            Destroy(floatingText, 2f);
            
            if(gameObject.tag != "Switch" && gameObject.tag != "Shield" && gameObject.tag != "Innocent")
            {
                SendMessage("UnderAttack");
            }
        }

        // If the hp is less or equal to 0 it dies
        if(hp <= 0)
        {
            
            // If the target is the player
            if (gameObject.layer == 9)
            {
                // Reset HP display to 0
                hpText.text = "Health: 0";
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
                // Destroys the game object
                if (anim != null)
                {
                    anim.SetTrigger("Death");

                    SendMessage("Dead");
                    Destroy(gameObject, 3);
                }
                else 
                {
                    Destroy(gameObject);
                }
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

    void GetCurrentFill()
    {
        float fillAmount = (float)hp / (float)maxHP;
        mask.fillAmount = fillAmount;
    }
}
