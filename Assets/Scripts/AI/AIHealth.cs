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
    private float pounceDamage = 5f;

    // Prefab to turn AI into a food node when it dies
    [SerializeField] GameObject foodPrefab;

    // Collider to detect if the players pounce hit the AI
    [SerializeField] SphereCollider pounceCollider;

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
        // If the object has been hit
        if (takeDamage.isHit == true && playerAttacks.quickOnCooldown == false)
        {
            // Check for damage taken from quick attack
            TakeDamage(quickScratchDamage);
        }
    }

    private void TakeDamage(float damageAmount)
    {
        currentAmount -= damageAmount;

        // Only deplete health if above 0
        if (currentAmount < 0)
        {
            // Clamp to 0
            currentAmount = 0;

            // AI 'turns into' a food node when it dies
            //Instantiate(foodPrefab, gameObject.transform);

            // Destroy the AI at the highest parent level
            Destroy(gameObject.transform.parent.transform.parent.gameObject);
        }

        // Save prevoius amount to new current amount
        previousAmount = currentAmount;

        // Update health visuals
        UpdateDamageVisual();

        takeDamage.OnMouseUp();
    }

    private void UpdateDamageVisual()
    {
        healthPoints[(int)currentAmount].color = depletedColour;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // If the AI enters the pounce collider
        if (collision.gameObject.name == "Pounce Collider")
        {
            // AI takes damage from the pounce attack
            TakeDamage(pounceDamage);
        }
    }
}
