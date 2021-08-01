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
    private float currentAmount;
    private float totalAmount;

    // Colour vars
    private Color originalColour;
    private Color depletedColour = Color.black;

    // Access to player movement script
    private PlayerMovement playerMovement;

    // Depletion rate increase bool
    private bool isIncreased = false;

    void Start()
    {
        // Set full resource amounts and original colour
        totalAmount = resources.Length;
        originalColour = resources[0].color;
        currentAmount = totalAmount;
        originalDepletionRate = depletionRate;

        // Get player movement script
        playerMovement = gameObject.GetComponent<PlayerMovement>();
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
            // Clamp to 0
            currentAmount = 0;
        }

        // Update UI visual
        UpdateVisual();

        // Check is player is sprinting and adjust depletion rate accordingly
        SprintDepletionIncrease();
    }

    private void UpdateVisual()
    {
        // Convert current amount into a %
        float amountPercent = currentAmount / totalAmount;

        // Take the number of the resource and put it under 1 to find out how much percent is one resource worth
        float singleResourcePercent = 1 / totalAmount;

        // If resources are depleted enough to show on 'meter'
        if (amountPercent < totalAmount - singleResourcePercent)
        {
            // Get how many of the resource icons should be filled in
            float visualRecourceAmount = amountPercent * totalAmount;

            // Once the resource hits 0
            if (currentAmount <= 0)
            {
                // Update UI visual to reflect the current amount
                resources[0].color = depletedColour;
            }
            // Convert to int (only becomes lower num when at that num or below)
            else if (visualRecourceAmount <= (int)visualRecourceAmount + 1 && (int)visualRecourceAmount + 1 != totalAmount)
            {
                // Update UI visual to reflect the current amount
                resources[(int)visualRecourceAmount + 1].color = depletedColour;
            }
        }
    }

    private void SprintDepletionIncrease()
    {
        // If the player is sprinting
        if (playerMovement.isSprinting == true && isIncreased == false)
        {
            // Increase the depletion rate
            depletionRate += 0.01f;

            // Depletion rate has been increased
            isIncreased = true;
        }
        // If the player is not sprinting
        else if (playerMovement.isSprinting == false && isIncreased == true)
        {
            // Return depletion rate to normal
            depletionRate = originalDepletionRate;

            // Depletion rate has been decreased
            isIncreased = false;
        }
    }

}
