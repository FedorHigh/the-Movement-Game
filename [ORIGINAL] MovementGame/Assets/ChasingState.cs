using UnityEngine;
using CustomClasses;
using UnityEngine.AI;

public class ChasingState : State
{
    public NavMeshAgent agent;
    public Entity host;
    public float speed = -1, angular = -1, acceleration = -1;
    public bool nullify = false;
    float saved, sangular, sacceleration;
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
        agent.speed = saved;
        agent.angularSpeed = sangular;
        agent.acceleration = sacceleration;
        if (nullify)
        {
            agent.speed = 0;
            agent.angularSpeed = 0;
            agent.acceleration = 0;
        }
    }
    public override void Enter(string info = "")
    {
        base.Enter(info);
        saved = agent.speed;
        sangular = agent.angularSpeed;
        sacceleration = agent.acceleration;
        if (speed != -1) agent.speed = speed;
        if (angular != -1) agent.angularSpeed = angular;
        if (acceleration != -1) agent.acceleration = acceleration;
        
        
    }
}
