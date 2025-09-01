using UnityEngine;

public class spinning : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.angularVelocity = new Vector3(0, speed, 0);
    }
}
