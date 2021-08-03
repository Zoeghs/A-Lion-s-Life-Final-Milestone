using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDepletion : MonoBehaviour
{
    // Array of recource to deplete (UI Images)
    [SerializeField] Image[] resources;

    // Resource nums
    [SerializeField] float depletionRate;
    private float originalDepletionRate;
    [HideInInspector] public float currentAmount;
    private float totalAmount;

    // Colour vars
    private Color originalColour;
    private Color depletedColour = Color.black;

    // Access to player movement script
    private PlayerMovement playerMovement;

    // Bool to tell if resources have been increased
    [HideInInspector] public bool hasIncreased = false;

    // Rate increase bools
    private bool sprintIncrease = false;
    private bool jumpIncrease = false;

    void Start()
    {
        // Set full resource amounts and original colour
        totalAmount = resources.Length;
        originalColour = resources[0].color;
        currentAmount = totalAmount;
        originalDepletionRate = depletionRate;

        // Get player movement script
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        DepletionOverTime();
    }

    private void DepletionOverTime()
    {
        // Take the total time to deplete and subtract the depletion rate
        currentAmount = currentAmount - (depletionRate * Time.deltaTime);

        // Only deplete if above 0
        if (currentAmount < 0)
        {
            // Clamp to 0 (min)
            currentAmount = 0;
        }

        // Update UI visual
        UpdateVisual();

        // Check is player is sprinting or jumping and adjust depletion rate accordingly
        SprintIncrease();
        JumpIncrease();
    }

    private void UpdateVisual()
    {
        // Convert current amount into a %
        float amountPercent = currentAmount / totalAmount;

        // Take the number of the resource and put it under 1 to find out how much percent is one resource worth
        float singleResourcePercent = 1 / totalAmount;

        // Get how many of the resource icons should be filled in
        float visualRecourceAmount = amountPercent * totalAmount;

        // If resources are depleted enough to show on 'meter'
        if (amountPercent < totalAmount - singleResourcePercent)
        {
            // Once the resource hits 0
            if (currentAmount <= 0)
            {
                // Update UI visual to reflect the current amount
                resources[0].color = depletedColour;
            }
            // Convert to int (only becomes lower num when at that num or below)
            else if (visualRecourceAmount <= (int)visualRecourceAmount + 1 && (int)visualRecourceAmount + 1 != totalAmount && hasIncreased == false)
            {
                // Update UI visual to reflect the current amount
                resources[(int)visualRecourceAmount + 1].color = depletedColour;
            }
        }

        // If the resource amount has increased
        if (hasIncreased == true)
        {
            // Do not increase more than the total amount
            if (currentAmount > totalAmount)
            {
                // Clamt to total amount (max)
                currentAmount = totalAmount;
            }

            // If the last point is not coloured
            if (resources[(int)totalAmount - 1].color != originalColour)
            {
                // Colour nessessary point
                resources[(int)visualRecourceAmount].color = originalColour;
            }

            // Reset bool
            hasIncreased = false;
        }
    }

    private void SprintIncrease()
    {
        // Create increase amount
        float amount = 0.01f;

        // If the player is sprinting
        if (playerMovement.isSprinting == true && sprintIncrease == false)
        {
            // Increase the depletion rate
            depletionRate += amount;

            // Depletion rate has been increased
            sprintIncrease = true;
        }
        // If the player is not sprinting
        else if (playerMovement.isSprinting == false && sprintIncrease == true)
        {
            // Return depletion rate to normal
            depletionRate -= amount;

            // Depletion rate has been decreased
            sprintIncrease = false;
        }
    }

    private void JumpIncrease()
    {
        // Create increase amount
        float amount = 0.01f;

        // If the player is sprinting
        if (playerMovement.isJumping == true && jumpIncrease == false)
        {
            // Increase the depletion rate
            depletionRate += amount;

            // Depletion rate has been increased
            jumpIncrease = true;
        }
        // If the player is not sprinting
        else if (playerMovement.isJumping == false && jumpIncrease == true)
        {
            // Return depletion rate to normal
            depletionRate -= amount;

            // Depletion rate has been decreased
            jumpIncrease = false;
        }
    }
}
