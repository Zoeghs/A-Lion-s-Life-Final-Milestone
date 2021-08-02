using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    // Get camera to look at
    [SerializeField] Camera mainCamera;

    // Access to damage script
    [SerializeField] AITakeDamage takeDamage;

    // Array of health to deplete (UI Images)
    [SerializeField] Image[] healthPoints;

    // Resource nums
    private float currentAmount;
    private float totalAmount;

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
    }

    void Update()
    {
        // Have UI look at the player's camera
        FollowCam();

        // Check for damage taken
        TakeDamage();
    }

    private void FollowCam()
    {
        // Point health canvas at the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);

    }

    private void TakeDamage()
    {
        // If the object has been hit
        if(takeDamage.isHit == true)
        {
            currentAmount -= quickScratchDamage;
        }

        // Update health visuals
        UpdateDamageVisual();
    }

    private void UpdateDamageVisual()
    {
        //healthPoints[(int)currentAmount].color = depletedColour;

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
                healthPoints[0].color = depletedColour;
            }
            // Convert to int (only becomes lower num when at that num or below)
            else if (visualRecourceAmount <= (int)visualRecourceAmount + 1 && (int)visualRecourceAmount + 1 != totalAmount)
            {
                // Update UI visual to reflect the current amount
                healthPoints[(int)visualRecourceAmount + 1].color = depletedColour;
            }
        }
    }
}
