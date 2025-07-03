using UnityEngine;
using CustomClasses;
using UnityEngine.AI;

public class ChasingState : State
{
    public NavMeshAgent agent;
    public Entity host;
    public override void Start() { 
        base.Start();
        if(agent == null) agent = GetComponent<NavMeshAgent>();
        if(host == null) host = GetComponent<Entity>();
    }
    public void Update()
    {
        if (!active) return;
        //Debug.Log(host.TargetObj.transform.position);
        agent.destination = host.TargetObj.transform.position;
        //Debug.Log(agent.destination);
        if((agent.destination - transform.position).magnitude <= agent.stoppingDistance) host.DoLookAtTarget();
    }
    public override void Exit(string info = "")
    {
        base.Exit(info);
        agent.destination = transform.position;
    }
}
