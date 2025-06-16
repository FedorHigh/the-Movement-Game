using UnityEngine;
using CustomClasses;
using UnityEngine.AI;

public class ChasingState : State
{
    public NavMeshAgent agent;
    public Entity host;
    public override void Start() { 
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        host = GetComponent<Entity>();
    }
    public void Update()
    {
        if (!active) return;
        agent.destination = host.TargetObj.transform.position;
        if((agent.destination - transform.position).magnitude <= agent.stoppingDistance) host.DoLookAtTarget();
    }
    public override void Exit(string info = "")
    {
        base.Exit(info);
        agent.destination = transform.position;
    }
}
