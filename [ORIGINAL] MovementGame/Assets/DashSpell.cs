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
        Dash(key, CD[0], 0);
    }
    public override void HeavyCast(KeyCode key)
    {
        
        //player.dashing = true;
        ready = false;
        CDleft = CD[1];
        CDset = CDleft;
        curKey = key;
        charge = minCharge;
        charging = true;
    }
    public override void ReleaseCharge() {

        player.queued = false;
        player.queuedCast = null;
        if (player.currentAbility != null) player.currentAbility.ability.Reset();

        Dash(curKey, CD[1], 1);
    }
    public override void LightCast(KeyCode key)
    {
        if (player.currentAbility != null) player.currentAbility.ability.Reset();

        ready = false;
        CDleft = CD[2];
        CDset = CDleft;
        //Debug.Log("light cast dash");
        
        //rb.linearVelocity = Vector3.zero;
        DashSpline s = splines[0];
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
        rb.AddForce(appliedVeclocity*lightBoost, ForceMode.Impulse);
    }
}
