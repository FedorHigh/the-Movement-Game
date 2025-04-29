using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class WanderAround : Action
{
    public Entity target;
    public NavMeshAgent agent;
    public float distance, delay, speed;
    float tmp;
    public bool waitForStop = true;
    public Transform center;
    Vector3 cntr;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        if(center==null) center = transform;
        cntr = center.position;
        cntr.y = transform.position.y;
        //wander();
    }
    public override bool StartAction() {
        //if (!enabled) return;
        //Debug.Log("Wandering...");
        if (!base.StartAction()) return false;
        Vector3 vec = new Vector3(Random.value, 0, Random.value).normalized * (distance * Random.value);
        agent.speed = speed;
        agent.destination = (cntr) + vec;
        //Debug.Log((cntr) + vec);
        return true;
        
    }
}
