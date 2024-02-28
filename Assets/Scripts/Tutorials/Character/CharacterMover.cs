using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    public Transform cam;

    public float mass = 200; // The character's mass, used for colliding rigid body collisions.

    public float speed = 10;
    public float jumpHeight = 4; // How high the character can jump.

    private Vector3 m_hitDirection;

    private Vector2 m_moveInput = Vector2.zero;
    private bool m_jumpInput = false;
    private bool m_crouchInput = false;

    private bool m_isCrouching = false;

    [SerializeField] private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private bool m_isGrounded = false;

    CharacterController m_cc = null;
    Animator m_animator;

    private void Start()
    {
        m_cc = GetComponent<CharacterController>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Get input.
        m_moveInput.x = Input.GetAxisRaw("Horizontal");
        m_moveInput.y = Input.GetAxisRaw("Vertical");
        m_jumpInput = Input.GetButton("Jump");
        m_crouchInput = Input.GetKey(KeyCode.LeftControl);

        m_animator.SetFloat("Forwards", m_moveInput.y);
        m_animator.SetBool("Jump", !m_isGrounded);
        m_animator.SetBool("Crouch", m_isCrouching);
    }

    private void FixedUpdate()
    {
        Vector3 delta;

        // Get camera directions.
        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;

        // Calculate movement vector based on camera's transforms and adjusted to player's speed.
        delta = (m_moveInput.x * camRight + m_moveInput.y * camForward) * speed;

        if (m_isGrounded || m_moveInput.x != 0.0f || m_moveInput.y != 0.0f) // On ground and recieving player input.
        {
            // Apply movement vector to velocity on player input or on ground.
            m_velocity.x = delta.x;
            m_velocity.z = delta.z;
        }
        if (m_jumpInput && m_isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight); // Calculate jump velocity using the fourth kinematic formula.
            m_velocity.y = jumpVelocity; // Do jump.
        }

        if (m_isGrounded && m_velocity.y < 0.0f)
            m_velocity.y = 0.0f; // Reset vertical velocity after falling to ground.
        m_velocity += Physics.gravity * Time.fixedDeltaTime; // Do gravity.

        if (!m_isGrounded)
            m_hitDirection = Vector3.zero; // Reset hit direction when in air.
        if (m_moveInput.x == 0.0f && m_moveInput.y == 0.0f) // No player input.
        {
            // Slide character off of object if standing on edge.
            Vector3 horizontalHitDirection = m_hitDirection;
            horizontalHitDirection.y = 0.0f;
            float displacement = horizontalHitDirection.magnitude;
            if (displacement > 0.0f)
                m_velocity -= 0.2f * horizontalHitDirection / displacement;
        }

        // Do crouch.
        if (m_crouchInput && m_isGrounded && m_moveInput.x == 0.0f && m_moveInput.y == 0.0f)
        {
            m_isCrouching = true;
        }
        else
        {
            m_isCrouching = false;
        }

        m_cc.Move(m_velocity * Time.fixedDeltaTime); // Apply velocity to object.
        transform.forward = camForward; // Match object's forward axis to the camera direction.

        m_isGrounded = m_cc.isGrounded; // Store whether the object is grounded or not on this frame.
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_hitDirection = hit.point - transform.position; //

        if (hit.rigidbody)
        {
            // Apply force to the object the character has collided with if it has a rigid body attached.
            hit.rigidbody.AddForceAtPosition(m_velocity * mass, hit.point); // Newton's second law, force = mass * acceleration.
        }
    }
}
