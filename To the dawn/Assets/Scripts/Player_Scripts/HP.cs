using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private TextMeshProUGUI hpText = default;
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
        }
        // If not a pop up appears on the target location
        else
        {
            // If it is damaged
            if (decrease > 0)
            {// Spawns the Text Object
                floatingText = Instantiate(dmgTextPrefab, transform.position,
                    Quaternion.FromToRotation(transform.position, player.transform.position), transform);

                // Updates the text according to the value the enemy lost
                floatingText.text = decrease.ToString();
            }
            // If the target is a switch
            else if(gameObject.tag == "Switch")
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
            // Destroys the game object
            if (anim != null)
            { 
                anim.SetTrigger("Death");
                Destroy(gameObject, 3);
            }
            else Destroy(gameObject);


            // If the target is the player
            if (gameObject.layer == 9)
            {
                // Spawns the Death Screen
                deathScreen.SetActive(true);
                // Stops game Time
                Time.timeScale = 0f;
                // Unlocks Mouse movement
                Cursor.lockState = CursorLockMode.None;
            }
            // If not the player...
            else if (anim == null || !anim.GetBool("Died"))
            {
                // ... Updates the player Kill Streak and stops the enemy from shooting
                Destroy(gameObject.GetComponent<ACBasicEnemy>());
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

    public void Death()
    {
        anim.SetBool("Died", true);
    }
}
