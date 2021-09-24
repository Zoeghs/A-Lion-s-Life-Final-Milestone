using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttacks : MonoBehaviour
{
    // Access to camera
    [SerializeField] Camera mainCamera;

    // Camera FOV
    private float originalFOV;

    // Cooldown times for attacks (seconds)
    private float quickScratchCooldown = 0f;

    // Cooldown bools
    [HideInInspector] public bool quickOnCooldown = false;

    // Attack visuals
    [SerializeField] Text quickCooldownVis;

    // Player movement sctipt var
    private PlayerMovement playerMovement;

    // Pounce UI
    [SerializeField] GameObject pounceUI;

    // Access to Slide script
    [SerializeField] Slide leftSlide;
    [SerializeField] Slide rightSlide;

    // Direction empty based off camera
    [SerializeField] Transform directionEmpty;

    // Bool for allowing pounce to function
    private bool canPounce = true;

    // Collider to detect if the pounce hit anything
    [SerializeField] SphereCollider pounceCollider;

    void Start()
    {
        // Get movement script
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        // Turn pounce UI off by default
        pounceUI.SetActive(false);

        // Save original FOV
        originalFOV = mainCamera.fieldOfView;

        // Turn off pounce collider
        pounceCollider.enabled = false;
    }

    void Update()
    {
        // Check for quick scratch
        QuickScratch();

        // Check for quick scratch cooldown
        Cooldown();

        // Check for sneaking (pounce preparation)
        Sneak();
    }

    #region Quick Scratch
    private void QuickScratch()
    {
        // If the player left clicks and right click is not being held and not on cooldown
        // Player also cannot be super sprinting to attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) == false && quickOnCooldown == false && playerMovement.moveSpeed <= playerMovement.sprintSpeed)
        {
            // Do a quick scratch
            print("Quick Scratch");

            // Begin cooldown
            quickOnCooldown = true;
            quickScratchCooldown = 3f;

            // vv Add visuals for quick scratch here vv
        }
    }

    private void Cooldown()
    {
        if (quickScratchCooldown > 0 && quickOnCooldown == true)
        {
            // Update UI text
            int roundTime = (int)quickScratchCooldown;
            quickCooldownVis.text = roundTime.ToString();

            // Subtract time per instance
            quickScratchCooldown -= Time.deltaTime;
        }
        // Once the timer hits 0
        else if (quickScratchCooldown <= 0)
        {
            // Ability no longer on cooldown
            quickOnCooldown = false;
        }
    }
    #endregion

    #region Pounce
    private void Sneak()
    {
        // When the player is holding right click
        if (Input.GetKeyDown(KeyCode.Mouse1) && canPounce == true)
        {
            // Slow down move speed
            playerMovement.moveSpeed = 4f;

            // Lock sprinting & jumping
            playerMovement.sprintLocked = true;
            playerMovement.jumpLocked = true;

            // Turn on pounce UI
            pounceUI.SetActive(true);

            // Change FOV to zoom/focus in
            mainCamera.fieldOfView = 20f;
        }
        // When the player is no longer holding right click
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // Restore move speed
            playerMovement.moveSpeed = playerMovement.originalMoveSpeed;

            // Unlock sprinting & jumping
            playerMovement.sprintLocked = false;
            playerMovement.jumpLocked = false;

            // Turn off pounce UI
            pounceUI.SetActive(false);

            // Reset t (time) in Slide script
            leftSlide.t = 0;
            rightSlide.t = 0;

            // Restore FOV
            mainCamera.fieldOfView = originalFOV;

            // Allow right click to be re-activated
            canPounce = true;

        }

        // If the player is holding down right click & releases left click
        if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyUp(KeyCode.Mouse0) && canPounce == true)
        {
            // Pounce at the force specified by the charge up
            Pounce();
        }
    }

    private void Pounce()
    {
        // Get player rigidbody
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        // Find Direction to launce player (based on where the player is looking)
        Vector3 pounceDir = directionEmpty.position - gameObject.transform.position;

        // Calculate the force the player will pounce based off the guage (t, time decimal)
        float force = leftSlide.t * 100f;

        // Lunge or pounce forward
        rb.AddForce(pounceDir * force, ForceMode.Impulse);

        // Turn on pounce collider
        pounceCollider.enabled = true;

        // Close pounce UI
        pounceUI.SetActive(false);

        // Restore FOV
        mainCamera.fieldOfView = originalFOV;

        // Player cannot pounce again until releasing the right mouse button
        canPounce = false;
    }
    #endregion
}
