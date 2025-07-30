using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Profiling;

public class Sticky : MonoBehaviour
{
    public Transform guide;
    public float force = 1;
    Transform player;
    Rigidbody rb;
    public Vector3 recorded, last, cur;
    public bool on = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /*
    void Update()
    {
        if (on) { 
            //player.position = player.position + (guide.position - recorded);
            rb.AddForce((player.position - guide.position)*force*Time.deltaTime*-1, ForceMode.Impulse);
            Debug.LogError((player.position - guide.position) * force * Time.deltaTime * -1);

            guide.position = player.position;
            recorded = guide.position;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            on = true;
            player = other.transform;
            rb = other.GetComponent<Rigidbody>();
            guide.position = player.position;
            recorded = guide.position;
            //other.gameObject.transform.parent = transform;
            /*
            ConstraintSource tmp = new ConstraintSource();
            tmp.sourceTransform = transform;
            tmp.weight = 1;
            other.GetComponent<ParentConstraint>().SetSource(0, tmp);
            other.GetComponent<ParentConstraint>().constraintActive = true;
            *//*
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            on = false;
            //other.gameObject.transform.parent = null;
            //other.GetComponent<ParentConstraint>().constraintActive = false;
        }
    }
    */
    void Update()
    {
        if (on) // Assuming a boolean flag for being on the platform
        {
            recorded = guide.GetComponent<Rigidbody>().linearVelocity;
            //Debug.LogError(recorded * Time.fixedDeltaTime * force);
            rb.MovePosition(rb.position + recorded * Time.deltaTime * force);
            /*
            cur = (recorded * Time.fixedDeltaTime);
            cur.y = 0;
            rb.linearVelocity += (cur - last) * force;
            Debug.LogError(cur - last);
            last = cur;
            */
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();
            on = true;
            
            
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            on = false;
        }
    }
}
