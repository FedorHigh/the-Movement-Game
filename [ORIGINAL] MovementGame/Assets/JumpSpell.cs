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

    public override void Start()
    {
        base.Start();
        combos = new CastInfo[1] { new CastInfo(GetComponent<LeapSpell>(), 0) };
    }

    public override void ResolveQueue(CastInfo curAbility, int cast)
    {
        if (cast == 0)
        {
            if (curAbility.Equals(combos[0]))
            {
                Debug.Log("Combo works!");
                Dash(curKey, CD[3], 2);
            }
            else Dash(curKey, CD[0], 0);
        }
        else if (cast == 2) LightCast(curKey);
        
    }

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
        if (player.dashing | !player.grounded) QueueCast(0);

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