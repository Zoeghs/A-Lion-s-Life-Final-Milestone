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
    [HideInInspector] public float moveSpeed = 6f;
    [HideInInspector] public float sprintSpeed = 12f;
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

    // Access to resource depletion scripts
    [SerializeField] ResourceDepletion foodDepletion;
    [SerializeField] ResourceDepletion waterDepletion;

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

        // Check if the player has enough resources to sprint
        bool canSprint = CheckResources();

        // Calculate and apply movement
        MovePlayer(canSprint);

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

    private void MovePlayer(bool canSprint)
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


        // If the player stops moving or does not have enough resources to sprint
        if (currentSpeed < 0.01f || canSprint == false)
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

            // If player is also sprinting and has not hit the speed cap
            if (isSprinting == true && moveSpeed < 32)
            {
                // Give them some forward momentum (super sprint)
                moveSpeed *= 2;

                // Allow resources to deplete faster
                foodDepletion.sprintIncrease = false;
                waterDepletion.sprintIncrease = false;

            }
        }
    }

    private bool CheckResources()
    {
        // Check if the player has enough resources to sprint
        // (Player needs to have enough of one or the other to be able to sprint, if one drops to 0, however, they will start to take damage)
        if (foodDepletion.amountPercent > foodDepletion.singleResourcePercent * 2 || waterDepletion.amountPercent > waterDepletion.singleResourcePercent * 2)
        {
            // Player has enough to sprint
            return true;
        }
        else
        {
            // Player does not have enough to sprint
            return false;
        }
    }
}
