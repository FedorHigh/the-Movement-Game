using UnityEngine;
using UnityEngine.Animations;

public class AssociatedVarsSpline : MonoBehaviour
{
    public TrailRenderer trail;
    public float duration;
    public Vector3 AppliedVeclocity;
    public bool debug = false;

    private void Update()
    {
        if (debug) {
            Debug.Log(GetComponent<ParentConstraint>().enabled);
        }
    }
}
