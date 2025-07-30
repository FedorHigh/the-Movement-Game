using UnityEngine;

public class Floating : MonoBehaviour
{
    Rigidbody rb;
    Vector3 vel;
    public float maxVal = 20, time = 2;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vel = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        vel.y += maxVal * (Time.deltaTime / time);
        if ((vel.y > maxVal && maxVal > 0) || (maxVal < 0 && vel.y < maxVal)) maxVal *= -1;
        rb.linearVelocity = vel;
    }
}
