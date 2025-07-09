using UnityEngine;
using CustomClasses;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class DashSpell : Ability, IAbility
{
    public GameObject box;
    public override void Cast()
    {
        Debug.Log("woosh");
        Dash(CD[0], 0);
        attackBoxes[0].SetActive(true);
    }
    public override void HeavyCast()
    {
        if (player.dashing)
        {
            QueueCast(1);
            return;
        }
        //player.dashing = true;
        ready = false;
        CDleft = CD[1];
        CDset = CDleft;
        charge = minCharge;
        charging = true;
    }
    public override void ReleaseCharge()
    {
        if (charge < maxCharge)
        {
            charging = true;
            return;
        }
        player.queued = false;
        player.queuedCast = null;
        if (player.currentAbility != null) player.abilities[player.currentAbility.ID].Reset();

        attackBoxes[1].SetActive(true);
        Dash(CD[1], 1);
    }
    public override void LightCast()
    {
        if (player.currentAbility != null)
        {
            //Debug.Log("RESET " + player.currentAbility.ID.ToString());
            player.abilities[player.currentAbility.ID].Reset();

        }

        Dash(CD[2], 2);
    }
}