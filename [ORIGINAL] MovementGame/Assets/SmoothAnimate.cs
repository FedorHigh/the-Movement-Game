using UnityEngine;
using UnityEngine.Splines;
using CustomClasses;
using UnityEngine.Animations;
public class SmoothAnimate : MonoBehaviour
{
    public SplineAnimate anim;
    public SplineContainer container;
    public ParentConstraint constraint;
    public Transform toMove;
    public float time, speed, duration, speedOverwrite;
    public bool playing;
    IAbility caller;
    StateMachine altCaller;
    public string method = "AnimOver";
    //public string resetMethod;
    //public 

    public void Start()
    {
        anim = GetComponent<SplineAnimate>();
        TryGetComponent<StateMachine>(out altCaller);
    }
    public void play(IAbility call = null)
    {
        speedOverwrite = 1;
        caller = call;
        //Debug.Log("Cast:");
        //Debug.Log(caller.GetID());
        if(constraint != null) constraint.enabled = false;
        anim.Container = container;
        time = 0;
        playing = true;
        anim.Duration = duration;
        duration = anim.Duration;
    }
    public void Reset()
    {
        playing = false;
        time = 0;
        if (constraint != null) constraint.enabled = true;
        if(toMove != null) toMove.position = transform.position;
        //Debug.Log(caller.ToString());
        if (caller != null) caller.Finish();
        if (altCaller != null) altCaller.Invoke(method, 0);
        //else Debug.Log("NULL CALLER");
    }
    void Update()
    {
        if (playing) {
            time += Time.deltaTime*speedOverwrite;
            if (time >= duration) { 
                time = duration;
                anim.ElapsedTime = time-0.00001f;
                Reset();
            }
            else anim.ElapsedTime = time;
        }
    }
}
