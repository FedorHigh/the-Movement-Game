using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{
    private NavMeshAgent agent;public Transform target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
}
