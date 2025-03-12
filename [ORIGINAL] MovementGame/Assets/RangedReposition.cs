using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class RangedReposition : MonoBehaviour
{
    public Entity entity;
    public GameObject target;
    public NavMeshAgent agent;
    public float mdForward, mdSide, ttlF, ttlS;
    public Vector3 shift;
    bool reverse;
    Vector3 vec;
    public void Reposition() {
        agent = GetComponent<NavMeshAgent>();
        TryGetComponent<Entity>(out entity);
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
    }
}
