using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class DashSpell : Ability, IAbility
{
    public override void Cast(KeyCode key)
    {
        Dash(key, BaseCD, 0);
    }
    public override void HeavyCast(KeyCode key)
    {
        ready = false;
        CDleft = HeavyCD;
        CDset = CDleft;
        curKey = key;
        charge = minCharge;
        charging = true;
    }
    public override void ReleaseCharge() {

        if (player.currentAbility != null) player.currentAbility.Reset();

        Dash(curKey, HeavyCD, 1);
    }
    public override void LightCast(KeyCode key)
    {
        ready = false;
        CDleft = LightCD;
        CDset = CDleft;
        //Debug.Log("light cast dash");

        //rb.linearVelocity = Vector3.zero;
        DashSpline s = splines[0];
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity*lightBoost, ForceMode.Impulse);
    }
}
