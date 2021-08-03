using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Vars for key inputs
    private float x;
    private float z;

    // Speed of the player
    private float originalMoveSpeed;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float sprintSpeed = 12f;
    [HideInInspector] public float currentSpeed;

    // Var to store character controller
    private CharacterController playerCont;

    // Player's velocity
    private Vector3 vel;

    // Player's gravity
    private float grav = -19.62f * 1.5f;

    // Ground check vars
    [SerializeField] Transform groundCheck;
    private float groundRaduis = 0.6f;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded = false;

    // Player jump height
    [SerializeField] float jumpHeight = 2f;

    // Sprinting & jumping mode detection
    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public bool isJumping = false;

    void Start()
    {
        // Get character controller component from player
        playerCont = gameObject.GetComponent<CharacterController>();

        // Save original move speed
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Check if player is grounded
        GroundCheck();

        // Get x and z axis input
        GetMovementInput();

        // Calculate and apply movement
        MovePlayer();

        // Check for jump
        Jump();

        // Apply gravity
        ApplyGravity();
    }

    private void GetMovementInput()
    {
        // Get x and z axis input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate where to move
        Vector3 move = transform.right * x + transform.forward * z;

        // If the player presses the sprinting key (toggle sprint)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // If the player is currently not sprinting
            if (moveSpeed == originalMoveSpeed)
            {
                // Change the move speed to the sprint speed
                moveSpeed = sprintSpeed;

                // Player is now sprinting
                isSprinting = true;

            }
            // If the player is currently sprinting
            else if (moveSpeed == sprintSpeed)
            {
                // Reset the move speed
                moveSpeed = originalMoveSpeed;

                // Player is no longer sprinting
                isSprinting = false;

            }

        }

        // Save position before movement application
        Vector3 lastPosition = transform.position;

        // Apply movement based on input
        playerCont.Move(move * moveSpeed * Time.deltaTime);

        // Calculate the current speed the player is moving
        currentSpeed = Vector3.Distance(lastPosition, transform.position);


        // If the player stops moving
        if (currentSpeed < 0.01f)
        {
            // Reset the move speed
            moveSpeed = originalMoveSpeed;

            // Player is no longer sprinting
            isSprinting = false;
        }
    }

    private void ApplyGravity()
    {
        // Add gravity to velocity
        vel.y += grav * Time.deltaTime;

        // Apply to character controller
        playerCont.Move(vel * Time.deltaTime);
    }

    private void GroundCheck()
    {
        // Do a sphere cast to check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRaduis, groundMask);

        // If player is grounded, reset velocity
        if (isGrounded && vel.y < 0)
        {
            // Set vel to something small so if check regesters before player is completely on the ground, they can still fall
            vel.y = -2f;

            // Player is no longer jumping
            isJumping = false;
        }
    }

    private void Jump()
    {
        // If jump key is pressed and player is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Player jumps
            vel.y = Mathf.Sqrt(jumpHeight * -2f * grav);

            // Player is now jumping
            isJumping = true;
        }
    }
}
