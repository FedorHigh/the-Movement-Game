using CustomClasses;
using UnityEngine;
using UnityEngine.AI;

public class RandomisedRepositionAction : Action
{
    public Vector3 min;
    public Vector3 max;
    Vector3 init, target;
    public float time;
    public bool moving;
    //public int trigger;
    public override void StartAction()
    {
        
        if (!base.PrepareToStart()) return;

        GetComponent<NavMeshAgent>().enabled = false;
        target = Random.value * (max - min) + min;
        init = transform.position;
        time = 0;
        moving = true;

    }
    public override void Update()
    {
        if (moving) {
            time += Time.deltaTime;
            if (time >= duration) {
                time = duration;
                moving = false;
            }

            transform.position = init + (target - init) * (time / duration);
        }
        base.Update();
    }
}
