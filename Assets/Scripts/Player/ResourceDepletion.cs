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
    //[SerializeField] float totalTimeToDeplete;
    private float currentAmount;
    private float totalAmount;

    // Colour vars
    private Color originalColour;
    private Color depletedColour = Color.black;

    void Start()
    {
        // Set full resource amounts and original colour
        totalAmount = resources.Length;
        originalColour = resources[0].color;
        currentAmount = totalAmount;
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

        print("Current:" + currentAmount);
        print("Ratio:" + currentAmount / totalAmount);

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Update the display to match new current amount (need to make this adaptable to a %)
        if (currentAmount == 5)
        {

        }
        else if (currentAmount <= 4 && currentAmount > 3)
        {
            resources[4].color = depletedColour;
        }
        else if (currentAmount <= 3 && currentAmount > 2)
        {
            resources[3].color = depletedColour;
        }
        else if (currentAmount <= 2 && currentAmount > 1)
        {
            resources[2].color = depletedColour;
        }
        else if (currentAmount <= 1 && currentAmount > 0)
        {
            resources[1].color = depletedColour;
        }
        else if (currentAmount <= 0)
        {
            resources[0].color = depletedColour;
        }
    }

}
