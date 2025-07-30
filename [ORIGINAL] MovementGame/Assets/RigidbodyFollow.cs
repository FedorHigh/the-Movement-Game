using UnityEngine;

public class RigidbodyFollow : MonoBehaviour
{
    public Transform target;
    public float force = 1;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError(target.position - transform.position);
        rb.linearVelocity = ((target.position - transform.position) * Time.deltaTime * force);
    }
}
