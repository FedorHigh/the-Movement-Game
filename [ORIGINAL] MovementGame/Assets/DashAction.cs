using UnityEngine;
using UnityEngine.AI;
using CustomClasses;

public class DashAction : Action
{
    //public GameObject attack;
    //public float slowSpeed, defSpeed, threshold, delay;
    //public bool slowDown, dash;
    public float dashStrength;
    //public MethodTrigger trigger;
    public NavMeshAgent agent;
    public Rigidbody rb;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void EndAction() {
        rb.isKinematic = true;
        agent.enabled = true;
        base.EndAction();
    }
 
    public override void StartAction()
    {
        Debug.Log("TRIED DASH");
        if (!base.PrepareToStart())
        {
            return;
        }
        rb.isKinematic = false;
        agent.enabled = false;
        rb.AddForce(transform.forward * dashStrength * rb.mass, ForceMode.Impulse);

        Debug.Log("DASHED");

        //return true;
    }
}
