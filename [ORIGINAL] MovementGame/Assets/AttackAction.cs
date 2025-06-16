using CustomClasses;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackAction : CustomClasses.Action
{
    public GameObject attack;
    public float slowSpeed, defSpeed, threshold, delay;
    public bool slowDown, dash;
    public float dist, dashStrength;
    public MethodTrigger trigger;
    public NavMeshAgent agent;
    public Rigidbody rb;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    
    public override void ResetReady()
    {
        if (trigger.inTrigger) waiting = true;
        base.ResetReady();
        
    }
    public void DoAttack() {
        UnityEngine.Debug.Log("did attack");
        attack.SetActive(true);
    }
    public override void StartAction()
    {
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
