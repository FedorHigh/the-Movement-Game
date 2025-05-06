using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AttackAction : Action
{
    public GameObject attack;
    public float slowSpeed, defSpeed, threshold, delay;
    public bool slowDown, dash;
    public float dist, dashStrength;
    public MethodTrigger trigger;
    public NavMeshAgent agent;
    public Rigidbody rb;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    public void resetMovement()
    {
        if (slowDown) agent.speed = slowSpeed;
        else agent.speed = defSpeed;
        agent.enabled = true;
        rb.isKinematic = true;
    }
    public override void ResetReady()
    {
        if (trigger.inTrigger) waiting = true;
        agent.speed = defSpeed;
        base.ResetReady();
        
    }
    public void TryAttack()
    {
        if (ready) StartAction();
    }
    public void DoAttack() {
        attack.SetActive(true);
    }
    public override bool StartAction()
    {
        if (!base.StartAction())
        {
            return false;
        }
        if (slowDown) agent.enabled = false;
        if (dash) {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * dashStrength * rb.mass, ForceMode.Impulse);
        }

        Invoke("resetMovement", CDset * 0.5f);

        ready = false;


        
        Invoke("resetReady", CDset);
        Invoke("DoAttack", delay);
        //moveSpeed = 0;
        //lookAtTarget = false;

        return true;
    }
}
