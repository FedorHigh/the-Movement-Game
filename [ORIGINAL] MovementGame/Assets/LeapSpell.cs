using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using UnityEngine.UIElements;


public class LeapSpell : Ability, IAbility
{
    public TrailRenderer XTrail;
    public bool charged, inHeavy;
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
    }
    public override void ReleaseCharge() {
        if (charge < maxCharge) {
            charging = true;
            return;
        }
        anim.speedOverwrite = chargeBoost;
        splines[1].trail.emitting = false;
        XTrail.emitting = true;
        charged = true;

        DashSpline s = splines[1];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity * (chargeBoost-1), ForceMode.Impulse);
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Finish()
    {
        //Debug.Log("FINISHED LEAP");
        ResetVars();
        if (charged) {
            Debug.Log("BOOM");
        }
    }

    

    public override void Cast()
    {
        Dash(CD[0], 0);
    }
    

    public override void HeavyCast()
    {
        if (inHeavy & !charged) {
            charging = true;
            anim.speedOverwrite = 0.2f;
            charge = 0;

            return;
        }

        if (player.dashing) QueueCast(1);
        inHeavy = true;
        Dash(0, 1);
    }

    public override void LightCast()
    {
        //Debug.Log("pew");
        if (player.dashing | inHeavy) QueueCast(1);
        CDleft = CD[2];
        CDset = CD[2];
        ready = false;

        DashSpline s = splines[0];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity*-1, ForceMode.Impulse);
    }
}
