using UnityEngine;
using UnityEngine.AI;

public class targetSetter : MonoBehaviour
{
    public GameObject trg;
    public NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.enabled)agent.destination = trg.transform.position;
    }
}
