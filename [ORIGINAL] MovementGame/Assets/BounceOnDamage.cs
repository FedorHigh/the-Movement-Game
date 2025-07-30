using CustomClasses;
using CustomEvents;
using UnityEngine;
using UnityEngine.AI;

public class BounceOnDamage : MonoBehaviour
{
    public Vector3 force;
    public float multiplier = 1;
    public Entity entity;
    public Action action;
    public bool resetSpeed = true;
    Rigidbody rb;
    NavMeshAgent agent;
    private void Start()
    {
        if(entity==null)entity = GetComponent<Entity>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        entity.onHitEvent += Bounce;
    }
    public void Bounce(HitInfo tmp, float extra = 1) {
        if (action != null) action.StartAction();
        Vector3 vec = (transform.forward * force.z + transform.right * force.x + transform.up * force.y) * multiplier * extra;
        rb.AddForce(vec, ForceMode.VelocityChange);
        if (resetSpeed) agent.velocity = new Vector3();
        Debug.Log("bounced by " + vec);
    }
    public void Bounce(HitInfo tmp) {
        Bounce(tmp, 1);
    }
}
