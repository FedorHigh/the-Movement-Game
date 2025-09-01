using UnityEngine;
using CustomClasses;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using UnityEngine.UIElements;


public class LeapSpell : Ability, IAbility
{
    public TrailRenderer XTrail;
    public bool charged, inHeavy;
    public BezierKnot[] savedSpline;
    public SplineContainer savedTarget;
    public int savedIndex;
    public GameObject debugMarker1, debugMarker2, landingMarker;
    [SerializeField] private float maxDownwardDistance = 100f;
    LayerMask ground;
    void DisplaceNodes(SplineContainer targetContainer, int index) {
        //return;
        savedTarget = targetContainer;
        Spline target = targetContainer.Spline;
        savedSpline = target.ToArray();
        savedIndex = index;
        if (dashInfo[index].splineObj.transform.rotation.eulerAngles.z != 0) return;

        //Instantiate(debugMarker1, transform.position, transform.rotation);
        Vector3 start = transform.position + dashInfo[index].splineObj.transform.forward*target.ToArray()[2].Position.z;
        Vector3 initPos = start;

        start.y += (dashInfo[index].splineObj.transform.up*target.ToArray()[1].Position.y).y;

        //Instantiate(debugMarker2, start, transform.rotation);
        RaycastHit hit;
        Ray ray = new Ray(start, Vector3.down);
        if (Physics.Raycast(ray,out hit, maxDownwardDistance, ground, QueryTriggerInteraction.Ignore)) { 
            float descent = start.y - hit.point.y;
            Vector3 debugVector = start;
            debugVector.y -= descent;
            descent = transform.position.y - debugVector.y;
            descent -= 0.5f;

            //Vector3 descentVector = splines[index].splineObj.transform.rotation * transform.up * -1 * descent;
            Vector3 descentVector = dashInfo[index].splineObj.transform.rotation * (initPos - hit.point) * -1;
            Debug.Log("Descent: " + descent.ToString() + " via " + descentVector.ToString() + " from " + start.ToString() + " to " + hit.point.ToString());

            BezierKnot tmpKnot;
            tmpKnot = target.ToArray()[1];

            tmpKnot.Position.x += (descentVector * 0.5f).x;
            tmpKnot.Position.y += (descentVector * 0.5f).y;
            tmpKnot.Position.z += (descentVector * 0.5f).z;

            target.SetKnot(1, tmpKnot);

            tmpKnot = target.ToArray()[2];

            tmpKnot.Position.x -= descentVector.x;
            tmpKnot.Position.y += descentVector.y;
            tmpKnot.Position.z -= descentVector.z;

            target.SetKnot(2, tmpKnot);
            

            Instantiate(landingMarker, debugVector, transform.rotation);
            debugVector.x = tmpKnot.Position.x;
            debugVector.y = tmpKnot.Position.y;
            debugVector.z = tmpKnot.Position.z;
            //Instantiate(debugMarker1, transform.position + debugVector, transform.rotation);

        }
    }
    void ResetNodes() {
        //return;
        for (int i = 0; i < savedSpline.Length; i++) { 
            savedTarget.Spline.SetKnot(i, savedSpline[i]);
        }
        dashInfo[savedIndex].spline = savedTarget;
    }
    public override void ResetVars()
    {
        //Debug.Log("RESET LEAP");
        
        base.ResetVars();
        //Debug.Log(splines[0].constraint.enabled.ToString() + " but " + splineObjs[0].GetComponent<ParentConstraint>().enabled.ToString());
        if (charged | inHeavy) {
            CDleft = CD[1];
            CDset = CD[1];
            ready = false;
        }
        charged = false;
        XTrail.emitting = false;
        inHeavy = false;
        ignoreDashing = false;
    }
    public override void ReleaseCharge() {
        
        if (charge < maxCharge) {
            charging = true;
            return;
        }
        base.ReleaseCharge();
        anim.speedOverwrite = chargeBoost;
        dashInfo[1].trail.emitting = false;
        XTrail.emitting = true;
        charged = true;

        DashInfo s = dashInfo[1];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity * (chargeBoost-1), ForceMode.Impulse);
    }
    public override void Start()
    {
        base.Start();
        ground = LayerMask.GetMask("Ground");
    }
    public override void Abort()
    {
        TryDischarge();
        base.Abort();
        ResetNodes();
    }
    public override void Finish()
    {
        //Debug.Log("FINISHED LEAP");
        TryDischarge();
        ResetVars();
        ResetNodes();
    }

    void TryDischarge() {
        if (charged)
        {
            Debug.Log("BOOM");
            doAttack(1, gameObject);
        }
    }

    public override void Cast()
    {
        base.Cast();
        DisplaceNodes(dashInfo[0].spline, 0);
        Dash(CD[0], 0);
        doAttack(0, gameObject);
    }

    public override void HeavyCast()
    {
        base.HeavyCast();
        if (inHeavy & !charged) {
            BeginCharging();
            anim.speedOverwrite = 0.2f;

            return;
        }

        inHeavy = true;
        ignoreDashing = true;
        DisplaceNodes(dashInfo[1].spline, 1);
        Dash(0, 1);
    }

    public override void LightCast()
    {
        return;
        //Debug.Log("pew");
        if (player.dashing | inHeavy)
        {
            QueueCast(1);
            return;
        }
        CDleft = CD[2];
        CDset = CD[2];
        ready = false;

        DashInfo s = dashInfo[0];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity*-1, ForceMode.Impulse);
    }
}
