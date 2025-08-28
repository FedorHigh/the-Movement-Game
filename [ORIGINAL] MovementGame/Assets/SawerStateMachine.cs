using CustomClasses;
using UnityEngine;
using UnityEngine.AI;

public class SawerStateMachine : CustomClasses.StateMachine
{
    BounceOnDamage bounce;
    NavMeshAgent agent;

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        bounce = GetComponent<BounceOnDamage>();
        base.Start();
        entity.onDetectionEvent += Detect;
    }

    public void Detect(GameObject tmp) {
        Trigger(0);
    }
    void Update()
    {
        bounce.multiplier = agent.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bounce.Bounce(null, 0.5f);
    }
}
