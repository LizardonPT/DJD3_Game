using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float MAX_FORWARD_ACCELERATION = 20.0f;
    private const float MAX_BACKWARD_ACCELERATION = 10.0f;
    private const float MAX_STRAFE_ACCELERATION = 15.0f;
    private const float JUMP_ACCELERATION = 500.0f;
    private const float GRAVITY_ACCELERATION = 30.0f;

    private const float MAX_FORWARD_VELOCITY = 5.0f;
    private const float MAX_BACKWARD_VELOCITY = 2.0f;
    private const float MAX_STRAFE_VELOCITY = 3.0f;
    private const float MAX_JUMP_VELOCITY = 30.0f;
    private const float MAX_FALL_VELOCITY = 50.0f;
    private CharacterController _controller;
    private Vector3 _acceleration;
    private Vector3 _velocity;
    private Vector3 _motion;
    private bool _jump;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _acceleration = Vector3.zero;
        _velocity = Vector3.zero;
        _motion = Vector3.zero;
        _jump = false;
    }

    void Update()
    {
        CheckForJump();
    }

    private void CheckForJump()
    {
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
            _jump = true;
    }

    void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        _acceleration.z = Input.GetAxis("Forward");

        if (_acceleration.z > 0)
            _acceleration.z *= MAX_FORWARD_ACCELERATION;
        else
            _acceleration.z *= MAX_BACKWARD_ACCELERATION;

        _acceleration.x = Input.GetAxis("Strafe") * MAX_STRAFE_ACCELERATION;

        if (_jump)
        {
            _acceleration.y = JUMP_ACCELERATION;
            _jump = false;
        }
        else if (_controller.isGrounded)
        {
            _acceleration.y = 0f;
        }
        else
        {
            _acceleration.y = -GRAVITY_ACCELERATION;
        }
    }

    private void UpdateVelocity()
    {
        _velocity += _acceleration * Time.fixedDeltaTime;

        _velocity.z = _acceleration.z == 0f ? 0f : Mathf.Clamp(_velocity.z, -MAX_BACKWARD_VELOCITY, MAX_FORWARD_VELOCITY);
        _velocity.x = _acceleration.x == 0f ? 0f : Mathf.Clamp(_velocity.x, -MAX_STRAFE_VELOCITY, MAX_STRAFE_VELOCITY);
        _velocity.y = (_acceleration.y == 0f) ? -0.1f : Mathf.Clamp(_velocity.y, -MAX_FALL_VELOCITY, MAX_JUMP_VELOCITY);
    }

    private void UpdatePosition()
    {
        _motion = transform.TransformVector(_velocity * Time.fixedDeltaTime);
        _controller.Move(_motion);
    }
}
