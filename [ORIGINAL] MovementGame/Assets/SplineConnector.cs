using System;
using UnityEngine;
using UnityEngine.Splines;

public class SplineConnector : MonoBehaviour
{
    public Transform other;
    public SplineContainer spline;
    public BezierKnot knot;
    void Start()
    {
        spline = GetComponent<SplineContainer>();
        knot = spline.Spline.ToArray()[1];
    }
    void Update()
    {
        //knot.Position = transform.position - other.position;
        knot.Position = other.position - transform.position;
        spline.Spline.SetKnot(1, knot);
    }
}
