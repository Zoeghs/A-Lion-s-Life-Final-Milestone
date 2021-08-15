// This script is by AxiomaticUncertainty
// Can be found at: https://www.youtube.com/watch?v=T8pZiWQZ63g

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public float t = 0f;
    public SplineManager spline;

    void Start()
    {
        t = 0;
    }

    void Update()
    {
        this.transform.position = spline.getSplinePosition(t);
        t += Time.deltaTime / 10f;

        if (t > 1)
        {
            // Return back to 0 (restart to beginning of spline)
            t = 0;
        }
    }
}
