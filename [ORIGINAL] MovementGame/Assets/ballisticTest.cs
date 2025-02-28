using UnityEngine;

public class ballisticTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float g = 9.8f, delay, mx = -1000000, mn = 1000000, maxDist = 0;
    public float mult = 1, dist = 10, time, speed;
    public Vector3 vel;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.AddForce(transform.up * g * dist * mult, ForceMode.VelocityChange);
    }
   
    void RePush() {
        //time = speed / g;
        //dist = speed * speed / (2*g);
        //time = dist * 2 / speed;
        speed = Mathf.Sqrt(dist * 2 * g);
        time = speed / g;
        

        rb.linearVelocity = new Vector3(0, g * time * mult, 0);
        //rb.AddForce(transform.up * g * dist * mult, ForceMode.VelocityChange);
        //rb.AddForce(transform.up * 10, ForceMode.Impulse);
        //Invoke("RePush", delay);
    }
    // Update is called once per frame
    void Update()
    {
        vel = rb.linearVelocity;
        //rb.AddForce(transform.up * -1 * g, ForceMode.Acceleration);
        if (Input.GetKeyDown(KeyCode.Space)) {
            RePush();
        }
        mx = Mathf.Max(mx, transform.position.y);
        mn = Mathf.Min(mn, transform.position.y);
        maxDist = mx - mn;
    }
}
