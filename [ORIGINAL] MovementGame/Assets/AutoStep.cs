using UnityEngine;

public class AutoStep : MonoBehaviour
{
    Rigidbody rb;
    public float up, forward;
    public MethodTrigger trigger;
    BetterController controller;
    public bool on = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<BetterController>();
    }
    public void Enable() { 
        on = true;
    }
    public void Disable() { on = false; }
    void Update()
    {
        if (on) {
            
            if (!trigger.inTrigger) { 
                rb.AddForce((transform.up*up + transform.forward*forward)*Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
