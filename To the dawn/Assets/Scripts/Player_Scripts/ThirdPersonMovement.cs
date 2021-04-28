using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float gravity = -18.81f;
    public float jumpHeight = 1f;

    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Verifys if the player in on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float targetAngle;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(gameObject.GetComponent<CMCameraPriority>().playerAim == true) 
        { // Movement while player is aiming
            targetAngle = PlayerRotation(direction);
            if (direction.magnitude >= 0.1f)
                PlayerMov(targetAngle);
        }
        else if (direction.magnitude >= 0.1f) 
        { // Movement witouth player aim
            targetAngle = PlayerRotation(direction);
            PlayerMov(targetAngle);
        }
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }
        
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if(isGrounded && velocity.y < 0)
            velocity.y = -6f; // Descend accel
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
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
