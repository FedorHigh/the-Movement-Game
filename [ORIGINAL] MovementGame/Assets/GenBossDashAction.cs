using System;
using UnityEngine;
using CustomClasses;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Splines;

public class GenBossDashAction : CustomClasses.Action
{
    public SmoothAnimate animate;
    public Animator animator;
    public float time = 0, length, delay, animDur = 0.5f;
    public bool turning;
    public GameObject obj, guide, box1, box2;
    public Quaternion init, target;
    public SplineContainer container;
    public string trigger = "dash";

    public override void StartAction()
    {
        if (!base.PrepareToStart()) return;
        base.StartAction();

        init = transform.rotation;
        target = Quaternion.LookRotation(guide.transform.position - transform.position, Vector3.up);
        time = 0;
        turning = true;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
        Invoke("Dash", delay);
    }

    public void Dash() {
        animate.container = container;
        animate.duration = animDur;
        animate.play();
        box1.SetActive(true);
    }

    public override void EndAction() {

        Instantiate(box2, transform.position, transform.rotation);

        base.EndAction();
    }

    public override void Update()
    {
        base.Update();
        if (turning)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(init, target, time / length);
            if (time >= length)
            {
                turning = false;
            }
        }
    }
}
