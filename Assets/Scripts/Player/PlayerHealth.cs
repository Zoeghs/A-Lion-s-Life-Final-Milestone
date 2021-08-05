using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Array of health points (UI Images)
    [SerializeField] Image[] healthPoints;

    // Resource nums
    private float totalHealth;
    private float currentHealth;

    // Bool to tell if health points have been increased
    [HideInInspector] public bool hasIncreased = false;

    // Damage nums
    private float resourceDamage = 0.15f;

    // Access to resource depletion scripts
    [SerializeField] ResourceDepletion foodDepletion;
    [SerializeField] ResourceDepletion waterDepletion;

    // Colour vars
    private Color originalColour;
    private Color depletedColour = Color.black;

    void Start()
    {
        // Set current and total health
        totalHealth = healthPoints.Length;
        currentHealth = totalHealth;

        // Save original colour
        originalColour = healthPoints[0].color;
    }

    void Update()
    {
        // Check for resource damage
        ResourceDamage();
    }

    private void TakeDamage(float amount, bool overTime)
    {
        // If the player needs to take damage over time
        if (overTime == true)
        {
            // Take damage over time
            currentHealth -= amount * Time.deltaTime;
        }
        // If the player needs to take instant damage
        else
        {
            // Player takes damage
            currentHealth -= amount;
        }

        // Clamp damage
        if (currentHealth < 0)
        {
            // Clamp to 0 (min)
            currentHealth = 0;

            // Player dies
            Dies();
        }
        else if (currentHealth > totalHealth)
        {
            // Clamp to total (max)
            currentHealth = totalHealth;
        }

        // Update UI to match current health
        UpdateDamageVisual();
    }

    private void UpdateDamageVisual()
    {
        // Update the UI to reflect the current health value

        // Convert current amount into a %
        float amountPercent = currentHealth / totalHealth;

        // Take the number of health points and put it under 1 to find out how much percent is one health point worth
        float singleResourcePercent = 1 / totalHealth;

        // Get how many of the resource icons should be filled in
        float visualRecourceAmount = amountPercent * totalHealth;

        // If health is depleted enough to show on 'meter'
        if (amountPercent < totalHealth - singleResourcePercent)
        {
            // If the last health point is depleted
            if (currentHealth <= 0)
            {
                // Update UI visual to reflect the current amount
                healthPoints[0].color = depletedColour;
            }
            // Convert to int (only becomes lower num when at that num or below)
            else if (visualRecourceAmount <= (int)visualRecourceAmount + 1 && (int)visualRecourceAmount + 1 != totalHealth && hasIncreased == false)
            {
                // Update UI visual to reflect the current amount
                healthPoints[(int)visualRecourceAmount + 1].color = depletedColour;
            }
        }

        // If the health amount has increased
        if (hasIncreased == true)
        {
            // If the last point is not coloured
            if (healthPoints[(int)totalHealth - 1].color != originalColour)
            {
                // Colour nessessary point
                healthPoints[(int)visualRecourceAmount].color = originalColour;
            }

            // Reset bool
            hasIncreased = false;
        }
    }

    private void Dies()
    {
        // Player dies
        print("You Died");
    }

    private void ResourceDamage()
    {
        // Player will take damage when at least one of their resource meters drops to 0
        // they take double damage if both are depleted

        // If both resource meters are depleted
        if (foodDepletion.amountPercent <= 0 && waterDepletion.amountPercent <= 0)
        {
            // Player takes double resource damage over time
            TakeDamage(resourceDamage * 2, true);
        }
        // If one of the meters is depleted
        else if (foodDepletion.amountPercent <= 0 || waterDepletion.amountPercent <= 0)
        {
            // Player takes resource damage over time
            TakeDamage(resourceDamage, true);
        }
    }
}
