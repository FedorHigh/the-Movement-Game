using UnityEngine;
using UnityEngine.Splines;
using Interfaces;
public class SmoothAnimate : MonoBehaviour
{
    public SplineAnimate anim;
    public SplineContainer container;
    public float time, speed, duration;
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
        caller = call;
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
        if(caller != null) caller.Reset();
    }
    void Update()
    {
        if (playing) {
            time += Time.deltaTime;
            if (time >= duration) { 
                time = duration;
                anim.ElapsedTime = time;
                Reset();
            }
            else anim.ElapsedTime = time;
        }
    }
}
