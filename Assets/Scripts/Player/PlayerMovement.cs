using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    // Vars for key inputs
    private float x;
    private float z;

    // Speed of the player
    [HideInInspector] public float originalMoveSpeed;
    [HideInInspector] public float moveSpeed = 8f;
    [HideInInspector] public float sprintSpeed = 12f;
    [HideInInspector] public float currentSpeed;
    private float maxSpeed;

    // Var to store character controller
    private Rigidbody playerRb;

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

    // Sprint & jump locks
    [HideInInspector] public bool sprintLocked = false;
    [HideInInspector] public bool jumpLocked = false;
    [HideInInspector] public bool velUpdateLocked = false;

    // Access to resource depletion scripts
    [SerializeField] ResourceDepletion foodDepletion;
    [SerializeField] ResourceDepletion waterDepletion;

    // Access to camera & swap script
    [SerializeField] Camera mainCam;
    private CameraSwap camSwap;

    // Collider to detect if the pounce hit anything
    [SerializeField] SphereCollider pounceCollider;

    // Sound effects
    [SerializeField] AudioSource walking;
    [SerializeField] AudioSource sprinting;
    [SerializeField] AudioSource sprinting2;
    [SerializeField] AudioSource sprinting4;

    // Sound effect bools
    private bool walkingPlaying = false;
    private bool sprintingPlaying = false;
    private bool sprinting2Playing = false;


    #endregion

    void Start()
    {
        // Get rigidbody component from player
        playerRb = gameObject.GetComponent<Rigidbody>();

        // Save original move speed
        originalMoveSpeed = moveSpeed;

        // Set max speed
        maxSpeed = (sprintSpeed * 2) * 2;

        // Get camera swap script
        camSwap = mainCam.GetComponent<CameraSwap>();
    }

    private void FixedUpdate()
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
        Vector3 move = playerRb.transform.right * x + playerRb.transform.forward * z;

        #region Sprint Toggle
        // If the player presses the sprinting key (toggle sprint) and sprinting is not locked
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprintLocked == false)
        {
            // If the player is currently not sprinting
            if (moveSpeed >= originalMoveSpeed && moveSpeed < sprintSpeed)
            {
                // Change the move speed to the sprint speed
                moveSpeed = sprintSpeed;

                // Player is now sprinting
                isSprinting = true;

            }
            // If the player is currently sprinting
            else if (moveSpeed >= sprintSpeed)
            {
                // Reset the move speed
                moveSpeed = originalMoveSpeed;

                // Player is no longer sprinting
                isSprinting = false;

            }
        }
        #endregion

        // If the update vel bool is not locked (the player is not pouncing)
        if (velUpdateLocked == false)
        {
            // Apply movement based on input
            playerRb.velocity = move * moveSpeed;
            print(playerRb.velocity.magnitude);

            // Check for sounds
            CheckForSounds();
        }


        // Calculate the current speed the player is moving
        currentSpeed = playerRb.velocity.magnitude;

        #region 3rd Person Mode
        // If the player is in 3rd person mode & is moving
        if (camSwap.inThirdPerson == true && currentSpeed > 0)
        {
            // Point the player in the direction the camera is facing & the player is moving
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.02f);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z)), 0.02f);
        }
        #endregion

        #region Force Stop Sprinting
        // If the player stops moving or does not have enough resources to sprint
        if (currentSpeed < 0.05f || canSprint == false)
        {
            // Reset the move speed
            moveSpeed = originalMoveSpeed;

            // Player is no longer sprinting
            isSprinting = false;
        }
        #endregion
    }

    private void CheckForSounds()
    {
        // VALUES ARE HARD CODED, NEED TO CHANGE AT SOME POINT

        if (walking != null)
        {
            // If the player is walking
            if (playerRb.velocity.magnitude > 0 && playerRb.velocity.magnitude <= 6 && walkingPlaying == false)
            {
                // Play walking sound
                walking.Play();

                // Sound is playing
                walkingPlaying = true;
            }
            // If the player is no longer walking
            else if ((playerRb.velocity.magnitude <= 0 || playerRb.velocity.magnitude >= 8f) && walkingPlaying == true)
            {
                // Stop playing the walking sound
                walking.Stop();

                // Sound is no longer playing
                walkingPlaying = false;
            }
            // If the player is sprinting
            else if (playerRb.velocity.magnitude >= 8 && sprintingPlaying == false)
            {
                // Play sprinting sounds
                sprinting.Play();

                // Sound is playing
                sprintingPlaying = true;
            }
            // If the player is no longer sprinting
            else if ((playerRb.velocity.magnitude < 8) && sprintingPlaying == true)
            {
                // Stop playing the sound
                sprinting.Stop();

                // Sound is no longer playing
                sprintingPlaying = false;
            }
            //// If the player is sprinting 2x
            //else if (playerRb.velocity.magnitude >= 16 && sprinting2Playing == false)
            //{
            //    // Play sprinting sounds
            //    sprinting2.Play();

            //    // Sound is playing
            //    sprinting2Playing = true;
            //}
            //// If the player is no longer sprinting 2x
            //else if ((playerRb.velocity.magnitude < 16 || playerRb.velocity.magnitude >= 32) && sprinting2Playing == true)
            //{
            //    // Stop playing the sound
            //    sprinting2.Stop();

            //    // Sound is no longer playing
            //    sprintingPlaying = false;
            //}
        }
    }

    private void ApplyGravity()
    {
        // Add gravity to velocity
        vel.y += grav * Time.deltaTime;

        // Apply to rigidbody
        playerRb.velocity = new Vector3(playerRb.velocity.x, vel.y, playerRb.velocity.z);
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

            // Turn off pounce collider
            pounceCollider.enabled = false;

            // Sart Updating player vel again
            velUpdateLocked = false;
        }
    }

    private void Jump()
    {
        // If jump key is pressed, player is grounded and jump is not locked
        if (Input.GetButton("Jump") && isGrounded && jumpLocked == false)
        {
            // Player jumps
            vel.y = Mathf.Sqrt(jumpHeight * -2f * grav);

            // Player is now jumping
            isJumping = true;

            // If player is also sprinting and has not hit the speed cap
            if (isSprinting == true && moveSpeed < maxSpeed)
            {
                // Give player a forward boost (lunge)
                playerRb.AddForce(transform.forward * 2, ForceMode.Impulse);

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
        if (foodDepletion.amountPercent > foodDepletion.singleResourcePercent * 4 || waterDepletion.amountPercent > waterDepletion.singleResourcePercent * 4)
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
