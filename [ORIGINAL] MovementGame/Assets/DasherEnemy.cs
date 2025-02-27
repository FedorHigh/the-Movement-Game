using Interfaces;
using UnityEngine;

public class DasherEnemy : Entity
{
    public GameObject attack;
    public float CDleft, CDset, slowSpeed, defSpeed, threshold;
    public bool attackReady;
    public float dist, dashStrength;
    public override void Update()
    {
        base.Update();
        dist = (TargetObj.transform.position - transform.position).magnitude;
        if (dist <= threshold && attackReady)
        {
            doAttack();
        }
    }
    public void resetMovement()
    {
        moveSpeed = slowSpeed;
        lookAtTarget = true;
    }
    public void resetAttack()
    {
        attackReady = true;
        moveSpeed = defSpeed;

    }
    public void doAttack()
    {
        rb.AddForce(transform.forward * dashStrength, ForceMode.Impulse);

        attackReady = false;
        Invoke("resetMovement", CDset * 0.7f);
        Invoke("resetAttack", CDset);
        attack.SetActive(true);
        moveSpeed = 0;
        lookAtTarget = false;
    }
    public override void Start()
    {
        base.Start();
        locateAnyTarget(100000);
    }

}
