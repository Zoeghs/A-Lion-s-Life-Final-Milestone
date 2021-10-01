using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplenishResource : MonoBehaviour
{
    #region Variables

    // Button prompt visuals
    [SerializeField] Image promptImage;
    [SerializeField] Text promptText;

    // Progress bar visuals
    [SerializeField] Image barBG;
    [SerializeField] Image barFG;

    // Pivot to scale bar foreground
    [SerializeField] Transform barFGPivot;

    // Percentage vars (total and current have been reversed becasue the foreground (black) needs to shrink rather than grow)
    private float totalPercentage = 0f;
    private float currentPercentage = 1f;
    private float originalCurrent;
    private float replenishRate;

    // Toggle to determine if the node is food or water (true for food false for water)
    [SerializeField] bool isFood;

    // Access to player's resource depletion script
    [SerializeField] ResourceDepletion playerResources;

    // How much of the resource the node gives
    [SerializeField] float restorationAmount;

    // Access to the player
    [SerializeField] Transform player;

    // How close the player has to be in order to collect resource
    private float distance = 5f;

    // Sound vars
    [SerializeField] AudioSource eatingSound;
    [SerializeField] AudioSource drinkingSound;
    private bool soundPlaying = false;

    #endregion

    void Start()
    {
        // Turn off prompt
        EnableTogglePrompt(false);

        // Turn off progress bar
        EnableToggleBar(false);

        // Save current percentage
        originalCurrent = currentPercentage;

        // Set progress bar scale to 0%
        //barFGPivot.localScale = new Vector3 (currentPercentage, barFGPivot.localScale.y, barFGPivot.localScale.z);

        // Set correct rates depending on the type of node
        if (isFood == true)
        {
            replenishRate = 0.2f;
            playerResources.depletionRate = 0.01f;
        }
        else
        {
            replenishRate = 0.4f;
            playerResources.depletionRate = 0.015f;
            playerResources.sprintAmount += 0.0005f;
            playerResources.jumpAmount += 0.0005f;
        }
    }

    private void OnMouseEnter()
    {
        // Only execute if within distance
        if (Vector3.Distance(player.position, gameObject.transform.position) <= distance)
        {
            // When hovering over the resource, give button prompt
            EnableTogglePrompt(true);
        }
        // If player is farther away than the required distance
        else
        {
            // Treat as mouse exit
            OnMouseExit();
        }

    }

    private void OnMouseExit()
    {
        // When no longer hovering over the resource, remove button prompt
        EnableTogglePrompt(false);

        // Disable progress bar
        EnableToggleBar(false);

        // Reset pivot scale
        ResetPivotScale();
    }

    private void OnMouseOver()
    {
        // Only execute if within distance
        if (Vector3.Distance(player.position, gameObject.transform.position) <= distance)
        {
            // If the replenish key is held down
            if (Input.GetKey(KeyCode.E))
            {
                Replenish();
            }
            // If the key is not held down
            else
            {
                // Re-enable prompt
                EnableTogglePrompt(true);

                // Disable progress bar
                EnableToggleBar(false);

                // Reset pivot scale
                ResetPivotScale();

                #region Sound

                // Stop playing sounds
                // If this is a food node
                if (isFood == true && soundPlaying == true)
                {
                    // Stop playing eating sound
                    eatingSound.Stop();

                    // Sound is no longer playing
                    soundPlaying = false;
                }
                // If this is a water node
                else if (isFood == false && soundPlaying == true)
                {
                    // Stop playing drinking sound
                    drinkingSound.Stop();

                    // Sound is no longer playing
                    soundPlaying = false;
                }

                #endregion
            }
        }
        // If player is farther away than the required distance
        else
        {
            // Treat as mouse exit
            OnMouseExit();
        }
    }

    private void Replenish()
    {
        // If the scale hits the total percentage
        if (currentPercentage <= totalPercentage)
        {
            // Add resource to desired resource bar
            playerResources.currentAmount += restorationAmount;
            playerResources.hasIncreased = true;

            // If the node is a food node
            if (isFood == true)
            {
                // Stop playing eating sound
                eatingSound.Stop();

                // Sound is no longer playing
                soundPlaying = false;

                // Resource is depleted
                Destroy(gameObject);
            }
            // If resource is meant to be reusable
            else
            {
                // Reset progress bar
                ResetPivotScale();
            }
        }
        // If the bar is not finished scaling
        else
        {
            // Disable button prompt
            EnableTogglePrompt(false);

            // Enable progress bar
            EnableToggleBar(true);

            // Add progress
            //currentPercentage += replenishRate * Time.deltaTime;

            // Subtract progress (decreases to reveal the bar behind, done in reverse with black/empty space as the foreground)
            currentPercentage -= replenishRate * Time.deltaTime;

            // Scale progress bar
            barFGPivot.localScale = new Vector3(currentPercentage, barFGPivot.localScale.y, barFGPivot.localScale.z);

            #region Sound

            // If the sound is not already playing
            // If this is a food node
            if (isFood == true && soundPlaying == false)
            {
                // Play eating sound
                eatingSound.Play();

                // Sound is playing
                soundPlaying = true;
            }
            // If this is a water node
            else if (isFood == false && soundPlaying == false)
            {
                // Play drinking sound
                drinkingSound.Play();

                // Sound is playing
                soundPlaying = true;
            }

            #endregion
        }
    }

    private void ResetPivotScale()
    {
        // Reset pivot scale
        barFGPivot.localScale = new Vector3(0, barFGPivot.localScale.y, barFGPivot.localScale.z);

        // Reset current percentage
        currentPercentage = originalCurrent;
    }

    private void EnableTogglePrompt(bool toggle)
    {
        promptImage.enabled = toggle;
        promptText.enabled = toggle;
    }

    private void EnableToggleBar(bool toggle)
    {
        barBG.enabled = toggle;
        barFG.enabled = toggle;
    }
}
