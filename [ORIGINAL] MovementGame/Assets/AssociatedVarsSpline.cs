using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class AssociatedVarsSpline : MonoBehaviour
{
    public TrailRenderer trail;
    public float duration;
    public Vector3 AppliedVeclocity, targetPosition;
    public ParticleHandle prtStart, prtEnd, prtMid;
    public bool debug = false;
    public AnimationCurve Xcurve = AnimationCurve.Constant(0, 1, 0), Ycurve = AnimationCurve.Constant(0, 1, 0), Zcurve = AnimationCurve.Constant(0, 1, 0);

}
