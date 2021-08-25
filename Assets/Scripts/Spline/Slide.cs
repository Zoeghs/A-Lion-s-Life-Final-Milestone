// This script is by AxiomaticUncertainty
// Can be found at: https://www.youtube.com/watch?v=T8pZiWQZ63g

// This script is to be put on ojects that move along the spline

// Modified by Zoe Pilgrim to reverse movement of the object along the spline, instead of resetting it to the starting point, once it reaches the end.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public float t = 0f;
    public SplineManager spline;

    // Direction bool
    private bool movePositive = true;

    // What to divide delta time by (speed: lower to go faster, higher to go slower)
    private float speedDivisor = 5f;

    void Start()
    {
        t = 0;
    }

    void Update()
    {
        this.transform.position = spline.getSplinePosition(t);

        // Find direction object needs to be moving in (positive of negative)
        FindDirection();

        // Move time according to the specified direction
        MoveTime();
    }

    private void FindDirection()
    {
        // When the object hits the end of the spline
        if (t >= 1)
        {
            // Decrease time, instead of increase time to make the object move in reverse
            movePositive = false;
        }
        // If the object hits the bottom on the spline
        else if (t <= 0)
        {
            // Increase time to move toawrds the end of the spline
            movePositive = true;
        }
    }

    private void MoveTime()
    {
        // If the object needs to move in a positive direction
        if (movePositive == true)
        {
            t += Time.deltaTime / speedDivisor;
        }
        // If the object needs to move in a negative direction
        else
        {
            t -= Time.deltaTime / speedDivisor;
        }
    }
}
