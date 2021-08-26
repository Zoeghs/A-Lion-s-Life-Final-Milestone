using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    // Access to other scripts
    [SerializeField] AITakeDamage takeDamage;
    [SerializeField] PlayerAttacks playerAttacks;

    // Array of health to deplete (UI Images)
    [SerializeField] Image[] healthPoints;

    // Resource nums
    private float currentAmount;
    private float totalAmount;
    private float previousAmount;

    // Colour vars
    private Color originalColour;
    private Color depletedColour = Color.black;

    // Damage nums: How much damage each attack does
    private float quickScratchDamage = 1f;

    void Start()
    {
        // Set full resource amounts and original colour
        totalAmount = healthPoints.Length;
        originalColour = healthPoints[0].color;
        currentAmount = totalAmount;
        previousAmount = currentAmount;
    }

    void Update()
    {
        // Check for damage taken
        TakeDamage();
    }

    private void TakeDamage()
    {
        // If the object has been hit
        if(takeDamage.isHit == true && playerAttacks.quickOnCooldown == false)
        {
            currentAmount -= quickScratchDamage;

            // Only deplete health if above 0
            if (currentAmount < 0)
            {
                // Clamp to 0
                currentAmount = 0;

                // vv Add death code in here vv
            }

            // Save prevoius amount to new current amount
            previousAmount = currentAmount;

            // Update health visuals
            UpdateDamageVisual();

            takeDamage.OnMouseUp();
        }
    }

    private void UpdateDamageVisual()
    {
        healthPoints[(int)currentAmount].color = depletedColour;
    }
}
