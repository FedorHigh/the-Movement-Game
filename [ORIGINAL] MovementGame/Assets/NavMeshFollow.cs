using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{
    private NavMeshAgent agent;public Transform target;
    public bool find;
    public string findName;
    void Start()
    {
        if (find) target = GameObject.Find(findName).transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
}
