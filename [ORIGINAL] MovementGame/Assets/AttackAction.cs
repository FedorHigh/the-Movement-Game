using CustomClasses;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackAction : CustomClasses.Action
{
    public GameObject attack;
    public float delay;
    public MethodTrigger trigger;
    public Vector3 push;
    Rigidbody rb;
    public bool doPush = false;
    bool savedKinematic = false;
    

    public override void Start()
    {
        if (!TryGetComponent<Rigidbody>(out rb)) doPush = false;
        base.Start();
        if(attack == null ) attack = gameObject;
    }
    
    public override void ResetReady()
    {
        if (trigger != null && trigger.inTrigger) waiting = true;
        base.ResetReady();
        
    }
    public void DoAttack() {
        UnityEngine.Debug.Log("did attack");
        attack.SetActive(true);
        if (!doPush) return;
        rb.isKinematic = false;
        rb.AddForce(push.x*transform.right + push.y*transform.up + push.z*transform.forward, ForceMode.VelocityChange);
    }
    public override void EndAction()
    {
        rb.isKinematic = savedKinematic;
        base.EndAction();
    }
    public override void StartAction()
    {
        savedKinematic = rb.isKinematic;
        UnityEngine.Debug.Log("tried attack");
        //StackTrace st = new StackTrace(true);
       // UnityEngine.Debug.Log(st.GetFrame(1).GetMethod().Name);
        if (!base.PrepareToStart())
        {
            return;
        }
        
        base.StartAction();

        ready = false;

        Invoke("DoAttack", delay);

        //return true;
    }
}
