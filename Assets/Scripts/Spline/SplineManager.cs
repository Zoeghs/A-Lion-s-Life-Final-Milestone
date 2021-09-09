// This script is by AxiomaticUncertainty
// Can be found at: https://www.youtube.com/watch?v=T8pZiWQZ63g

using System.Collections.Generic;
using UnityEngine;

public class SplineManager : MonoBehaviour
{
    [SerializeField] List<GameObject> verticies;
    [SerializeField] GameObject renderPrefab;

    private List<PiecewiseCubic> pieces = new List<PiecewiseCubic>();

    //[SerializeField] GameObject prevR;

    //[SerializeField] LineRenderer renderer;

    void Start()
    {
        
    }

    void Update()
    {
        pieces.Clear();

        for (int i = 0; i < verticies.Count - 1; i++)
        {
            pieces.Add(new PiecewiseCubic(verticies[i].transform.position, verticies[i + 1].transform.position, verticies[i].transform.up * verticies[i].GetComponent<Vertex>().magnitudeForward, verticies[i + 1].transform.up * verticies.Count));

            //renderer.SetPositions(getPoints(pieces));
        }
    }

    private class PiecewiseCubic
    {
        Vector3 a, b, c, d;

        public PiecewiseCubic(Vector3 x0, Vector3 xf, Vector3 dx0, Vector3 dxf)
        {
            a = dxf + dx0 + 2 * (x0 - xf);
            b = 3 * (xf - x0) - 2 * dx0 - dxf;
            c = dx0;
            d = x0;
        }

        public Vector3 calculatePoint(float t)
        {
            return a * Mathf.Pow(t, 3) + b * Mathf.Pow(t, 2) + c * t + d;
        }
    }

    private Vector3[] getPoints(List<PiecewiseCubic> pcs)
    {
        int numPts = pieces.Count * 50;
        Vector3[] pts = new Vector3[numPts];
        //renderer.positionCount = numPts;

        for (int i = 0; i < pieces.Count; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                pts[50 * i + j] = pieces[i].calculatePoint(j / 50f);
            }
        }

        return pts;
    }

    public Vector3 getSplinePosition(float t)
    {
        if (pieces.Count > 0)
        {
            float x = Mathf.Clamp01(t) * pieces.Count;

            return (int)t < 1 ? pieces[(int)x].calculatePoint(x - Mathf.Floor(x)) : verticies[verticies.Count - 1].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
