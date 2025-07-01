using UnityEngine;
using CustomClasses;
using System.ComponentModel;
public class WormEnemy : Entity
{
    int debug = 0;
    public float G, upG, downG, upBoost, downBoost, boostAmount;
    public bool AboveGround, locked;
    public GameObject guide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        //locateAnyTarget(100000);
        G = downG;
        AboveGround = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        rb.AddForce(transform.up * G, ForceMode.Force);
    }

    public void Flip() {
        locked = true;
        Debug.Log(debug);
        debug += 1;
        if (AboveGround)
        {
            AboveGround = false;
            G = upG;
            rb.AddForce(transform.up * downBoost, ForceMode.VelocityChange);
        }
        else 
        {
            AboveGround = true;
            G = downG;
            rb.AddForce(transform.up * upBoost, ForceMode.VelocityChange);
        }
        locked = false;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == guide && !locked)
        {
            locked = true;
            //rb.AddForce(transform.up * G * boostAmount, ForceMode.Impulse);
            Flip();
            locked = false;
        }
    }
}
