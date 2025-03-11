using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class WanderAround : MonoBehaviour
{
    public Entity target;
    public NavMeshAgent agent;
    public float distance, delay, speed;
    float tmp;
    public bool waitForStop = true;
    public Transform center;
    Vector3 cntr;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cntr = center.position;
        cntr.y = transform.position.y;
        wander();
    }
    public void wander() {
        Debug.Log("Wandering...");
        Vector3 vec = new Vector3(Random.value, 0, Random.value).normalized * (distance * Random.value);
        agent.speed = speed;
        agent.destination = (cntr) + vec;
        Debug.Log((cntr) + vec);
        Invoke("wander", delay);
    }
}
