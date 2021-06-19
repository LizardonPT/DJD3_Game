using UnityEngine;
using TMPro;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Animator interfaceAnim = default;
    [SerializeField] private TextMeshProUGUI dashChargeText = default;
    [SerializeField] private CharacterController controller = default;
    [SerializeField] private Transform cam = default;

    public float speed = 6f;
    [SerializeField] private float gravity = -1000000f;
    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private Transform groundCheck = default;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask = default;
    [SerializeField] private int maxCharges = default;
    [SerializeField] private int energyDash = 10;

    [SerializeField] private AudioClip dashSound;
    private AudioSource myAudio;

    private bool isGrounded;

    private int charges;
    private float timer;
    private float dashTimer;
    private float turnSmoothVelocity;
    private float dashCooldown = 3f;
    private float tempDashCooldown = 3f;

    private Vector3 moveDir;
    private Vector3 velocity;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        charges = maxCharges;
        dashChargeText.text = "Charges: " + maxCharges.ToString();
        myAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifys if the player in on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Prepare timers for dash
        timer += Time.deltaTime;
        dashTimer += Time.deltaTime;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float targetAngle;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(dashTimer<0.2f)
        {
            controller.Move(moveDir * (speed*4) * Time.deltaTime);
            velocity.y = 0;
        }
        // Movement while player is aiming
        else if(gameObject.GetComponent<CMCameraPriority>().playerAim)
        {
            targetAngle = PlayerRotation(direction);
            if (direction.magnitude >= 0.1f)
                PlayerMov(targetAngle);
        }
        // Movement witouth player aim
        else if (direction.magnitude >= 0.1f)
        {
            targetAngle = PlayerRotation(direction);
            PlayerMov(targetAngle);
        }

        // Dash
        if(Input.GetButtonDown("Dash") && charges > 0 && (gameObject.GetComponent<Energy>().energy - energyDash > 0))
        {
            dashTimer = 0;
            charges--;
            gameObject.GetComponent<Energy>().UpdateEnergy(energyDash);
            dashChargeText.text = "Charges: " + charges.ToString();
            myAudio.clip = dashSound;
            myAudio.Play();
            gameObject.GetComponent<CMCameraPriority>().InterruptAim();
            //CMCameraPriority.Interrup
        }
        else if (Input.GetButtonDown("Dash") && charges == 0)
        {
            interfaceAnim.SetTrigger("NoCharges");
        }
        else if (Input.GetButtonDown("Dash") && (gameObject.GetComponent<Energy>().energy - energyDash > 0))
        {
            interfaceAnim.SetTrigger("NoEnergy");
        }

        // Jump
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(charges == maxCharges)
        {
            timer = 0;
        }
        else if((charges < maxCharges) && (timer>= tempDashCooldown))
        {
            charges++;
            timer = 0;
            dashChargeText.text = "Charges: " + charges.ToString();
        }
    }

    private float PlayerRotation(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        return targetAngle;
    }

    private void PlayerMov(float targetAngle)
    {
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir * speed * Time.deltaTime);
    }

    public void RapidCharge(float bonus)
    {
        tempDashCooldown = dashCooldown * bonus;
    }
}
