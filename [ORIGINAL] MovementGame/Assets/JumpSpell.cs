using UnityEngine;
using CustomClasses;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using System.Data.SqlTypes;
using UnityEngine.UIElements;

public class JumpSpell : Ability, IAbility
{
    private BezierKnot tmpknot;
    public float defDist, addDist;
    public override void ResetVars()
    {
        player.slowFall = true;
        base.ResetVars();
        
        //Debug.Log("reset jump!");
    }
    public override void Start()
    {
        base.Start();
        combos = new CastInfo[1] { new CastInfo(2, 0) };
    }

    public override void ResolveQueue(CastInfo curAbility, int cast)
    {
        if (cast == 0)
        {
            if (curAbility.Equals(combos[0]))
            {
                Debug.Log("Combo works!");
                Dash(CD[3], 2);
            }
            else Dash(CD[0], 0);
        }
        else if (cast == 2) LightCast();
        else HeavyCast();
        
    }

    public override void ReleaseCharge() {
        //Debug.Log("releasing jump!");
        if (!player.grounded) {
            Invoke("ReleaseCharge", 0.1f);
            //Debug.Log("delayed");
            return;
        }

        player.slowFall = true;
        Instantiate(attackBoxes[1], transform.position, transform.rotation);
        DashSpline s = splines[1];
        tmpknot = s.spline.Spline.ToArray()[1];
        tmpknot.Position.y = defDist + (charge / maxCharge) * addDist;
        splines[1].spline.Spline.SetKnot(1, tmpknot);
        splines[1].addedVelocity *= (charge / maxCharge);
        splines[1].addedVelocity = s.addedVelocity * (charge / maxCharge);

        //if (player.currentAbility != null) player.currentAbility.Reset();
        player.dashing = false;
        Dash(CD[1], 1);

        splines[1].addedVelocity = s.addedVelocity;
    }
   
    public override void Cast()
    {
        if (player.dashing | !player.grounded) QueueCast(0);
        player.slowFall = true;

        Instantiate(attackBoxes[0], transform.position, transform.rotation);

        Dash(CD[0], 0);
    }

    public override void HeavyCast()
    {
        if (player.dashing) QueueCast(1);
        if (!player.grounded | player.dashing) return;
        Debug.Log("started charging jump");
        player.dashing = true;
        ready = false;
        charging = true;
        charge = 0;
    }

    public override void LightCast()
    {
        if (player.dashing) QueueCast(2);
        ready = false;
        CDleft = CD[2];
        CDset = CDleft;

        DashSpline s = splines[0];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity * lightBoost, ForceMode.Impulse);
    }

}