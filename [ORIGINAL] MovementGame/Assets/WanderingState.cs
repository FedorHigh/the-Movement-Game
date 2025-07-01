using UnityEngine;
using CustomClasses;
using UnityEngine.AI;

public class WanderingState : State
{
    WanderAround action;
    public override void Start()
    {
        base.Start();
        action = gameObject.GetComponent<WanderAround>();
    }

    public override void Enter(string info = "")
    {
        base.Enter();
        Debug.Log("wandering ");
        action.StartActionRepeating();
    }
    public override void Exit(string info = "")
    {
        base.Exit();
        action.Cancel();
        GetComponent<NavMeshAgent>().destination = transform.position;
    }
}
