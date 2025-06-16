using UnityEngine;
using UnityEngine.Splines;
using CustomClasses;
public class SmoothAnimate : MonoBehaviour
{
    public SplineAnimate anim;
    public SplineContainer container;
    public float time, speed, duration, speedOverwrite;
    public bool playing;
    IAbility caller;
    //public string resetMethod;
    //public 

    public void Start()
    {
        anim = GetComponent<SplineAnimate>();
    }
    public void play(IAbility call = null)
    {
        speedOverwrite = 1;
        caller = call;
        //Debug.Log("Cast:");
        //Debug.Log(caller.GetID());
        anim.Container = container;
        time = 0;
        playing = true;
        anim.Duration = duration;
        duration = anim.Duration;
    }
    public void Reset()
    {
        time = 0;
        playing = false;
        //Debug.Log(caller.ToString());
        if (caller != null) caller.Finish();
        else Debug.Log("NULL CALLER");
    }
    void Update()
    {
        if (playing) {
            time += Time.deltaTime*speedOverwrite;
            if (time >= duration) { 
                time = duration;
                anim.ElapsedTime = time;
                Reset();
            }
            else anim.ElapsedTime = time;
        }
    }
}
