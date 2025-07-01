using CustomClasses;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Splines;

public class JumpAction : Action
{
    public GameObject splineObj, guide, obj, box1, box2;
    public DashSpline s;
    public Animator animator;
    public SmoothAnimate animate;
    public string trigger = "jump";
    public float delay2, delay3, length, animDur = 2f;
    public Quaternion init, target;
    public float time = 0;
    public bool turning = false, teleportGuide = true;
    public SplineContainer container;

    public override void Start()
    {
        s = new DashSpline(splineObj);
        
        base.Start();
        //StartActionRepeating();
        
    }
    public override void Update()
    {
        base.Update();
        if (turning) { 
            time += Time.deltaTime;
            obj.transform.rotation = Quaternion.Slerp(init, target, time/length);
            if (time >= length) { 
                turning = false;
            }
        }
    }

    public override void StartAction()
    {
        if(!PrepareToStart()) return;

        
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
        if(teleportGuide)guide.transform.position = transform.position;


        Invoke("Action2", delay2);
        Invoke("Action3", delay3);
        Debug.Log("REPOSITIONED SPLINE");


    }
    public void Action2() {
        //obj.transform.LookAt(guide.transform.position);
        target = Quaternion.LookRotation(guide.transform.position - obj.transform.position, Vector3.up);
        init = obj.transform.rotation;
        time = 0;
        turning = true;
        //obj.transform.Rotate(0, -90, 0);
        //obj.transform.rotation = Quaternion.Euler(new Vector3( 0, obj.transform.rotation.y+90, 0 ));
    }
    public void Action3() {
        float tmp = s.constraint.GetSource(0).sourceTransform.position.y;

        Debug.Log("played jump!");
        BezierKnot end = s.spline.Spline.ToArray()[2];
        end.Position = 1*(guide.transform.position - transform.position);
        end.Position.y = transform.position.y - tmp;
        s.spline.Spline.SetKnot(2, end);

        Debug.Log("played jump!");
        BezierKnot mid = s.spline.Spline.ToArray()[1];
        mid.Position = 1*(guide.transform.position - transform.position) / 2;
        mid.Position.y = transform.position.y + 15 - tmp;
        s.spline.Spline.SetKnot(1, mid);
        animate.container = container;

        Debug.Log("played jump!");
        animate.duration = animDur;
        animate.play();
        Instantiate(box1, obj.transform.position, obj.transform.rotation);
    }
    public override void EndAction()
    {
        Instantiate(box2, obj.transform.position, obj.transform.rotation);
        base.EndAction();
    }
}
