using UnityEngine;
using UnityEngine.AI;

public class MovementAnimator : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    public string variable;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(variable, agent.velocity.magnitude > 0.1f);
    }
}
