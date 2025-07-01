using CustomClasses;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class RangedRepositionAction : Action
{
    public Entity entity;
    public GameObject target;
    public NavMeshAgent agent;
    public float mdForward, mdSide, ttlF, ttlS;
    public Vector3 shift;
    bool reverse;
    Vector3 vec;
    public override void StartAction() {
        if(!base.PrepareToStart()) return;
        entity = host;
        agent = GetComponent<NavMeshAgent>();
        //TryGetComponent<Entity>(out entity);
        target = entity.TargetObj;

        transform.LookAt(target.transform);
        reverse = false;
        
        ttlS = Random.value * mdSide + shift.x;
        ttlF = Random.value * mdForward + shift.z;

        if (Random.value >= 0.5f) reverse = true;
        if (reverse) ttlS *= -1;

        Debug.Log(ttlS);
        Debug.Log(ttlF);

        vec = ttlS * transform.right + ttlF * transform.forward;
        Debug.Log(vec);
        vec += (transform.position);
        agent.SetDestination(vec);
        //Debug.Log(vec);
        //return true;
    }
}
