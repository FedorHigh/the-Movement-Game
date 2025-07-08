using UnityEngine;
using UnityEngine.AI;

public class shrinking : MonoBehaviour
{
    NavMeshAgent agent;
    public float time = 3;
    float curTime = 0, maxScale;
    Vector3 scale;
    void Start()
    {
        scale = transform.localScale;
        maxScale = scale.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= time) Destroy(gameObject);
        scale.y = maxScale * (1-(curTime / time));
        transform.localScale = scale;
    }
}
