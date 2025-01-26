using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using System.Data.SqlTypes;
using UnityEngine.UIElements;

public class JumpSpell : Ability, IAbility
{
    private BezierKnot tmpknot;
    public float defDist, addDist;
    public override void ReleaseCharge() {
        if (!player.grounded) {
            //Invoke("ReleaseCharge", 0.1f);
            return;
        }
        DashSpline s = splines[1];
        tmpknot = s.spline.Spline.ToArray()[1];
        tmpknot.Position.y = defDist + (charge / maxCharge) * addDist;
        splines[1].spline.Spline.SetKnot(1, tmpknot);
        splines[1].addedVelocity *= (charge / maxCharge);
        splines[1].addedVelocity = s.addedVelocity * (charge / maxCharge);

        //if (player.currentAbility != null) player.currentAbility.Reset();

        Dash(curKey, CD[1], 1);

        splines[1].addedVelocity = s.addedVelocity;
    }
   
    public override void Cast(KeyCode key)
    {
        if (player.dashing | !player.grounded) return;

        Dash(key, CD[0], 0);
    }

    public override void HeavyCast(KeyCode key)
    {
        if (!player.grounded | player.dashing) return;
        player.dashing = true;
        ready = false;
        CDleft = maxCharge;
        CDset = CDleft;

        curKey = key;
        charging = true;
        charge = 0;
    }

    public override void LightCast(KeyCode key)
    {
        if (player.dashing) return;
        ready = false;
        CDleft = CD[2];
        CDset = CDleft;

        DashSpline s = splines[0];
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity * lightBoost, ForceMode.Impulse);
    }

}