using UnityEngine;
using CustomClasses;
using UnityEngine.AI;

public class MovementEasingAction : Action
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    //public Action action;
    public float firstSpeed = 0, secondSpeed, defSpeed, threshold = 1.5f;
    public override void StartAction()
    {
        if (!base.PrepareToStart())
        {
            Debug.Log("easing not ready");
            return;
        }
        //if (!action.ready)
        //{
        //    Debug.Log("attack not ready");
        //    return;
        //}

        defSpeed = agent.speed;
        //agent.enabled = false;
        agent.speed = firstSpeed;
        Invoke("resetMovement", threshold);
        //action.StartAction();

        //return true;
    }
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //duration = CDset;
    }
    public void resetMovement()
    {
        agent.speed = secondSpeed;
        //agent.enabled = true;
        rb.isKinematic = true;
    }
    public override void EndAction()
    {
        agent.speed = defSpeed;
        base.EndAction();
    }
    public override void Cancel()
    {
        base.Cancel();
        CancelInvoke("resetMovement");
    }
}
