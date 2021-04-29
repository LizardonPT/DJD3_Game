using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Animator interfaceAnim;
    [SerializeField] private TextMeshProUGUI dashChargeText;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float gravity = -18.81f;
    [SerializeField] private float jumpHeight = 1f;

    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private int maxCharges;
    [SerializeField] private int energyDash = 10;


    private bool isGrounded;
    private bool jump;

    private int charges;
    private float timer;
    private float jumpTimer;
    private float dashTimer;
    private float turnSmoothVelocity;
    private float dashCooldown = 3f;

    private Vector3 moveDir;
    private Vector3 velocity;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        charges = maxCharges;
        dashChargeText.text = maxCharges.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifys if the player in on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Prepare timers for dash
        timer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float targetAngle;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(dashTimer<0.2f)
        {
            controller.Move(moveDir * (speed*4) * Time.deltaTime);
            velocity.y = 0;
        }
        else if(jump)
        {
            if(vertical != 0f || horizontal != 0f)
            {
                controller.Move(moveDir * (speed*1.2f) * Time.deltaTime);
            }
            if(isGrounded && jumpTimer>= 0.1f)
            {
                jump = false;
            }
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
            dashChargeText.text = charges.ToString();
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
            jump = true;
            jumpTimer = 0;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if(!isGrounded && velocity.y < 0)
        {
            velocity.y = -4f; // Descend accel
        }

        if(charges == maxCharges)
        {
            timer = 0;
        }
        else if((charges < maxCharges) && (timer>= dashCooldown))
        {
            charges++;
            timer = 0;
            dashChargeText.text = charges.ToString();
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
}
