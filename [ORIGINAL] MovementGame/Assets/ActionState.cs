using CustomClasses;
using UnityEngine;

public class ActionState : State
{
    public Action action;
    public float duration;
    public int triggerId;
    public bool inherit = true;

    public override void Start()
    {
        base.Start();
        if (inherit && action!=null) { 
            duration = action.duration;
            //triggerMethod = action.name + "EndTrigger";
        }
    }
    public override void Enter(string info = "")
    {
        base.Enter(info);
        if(action != null) action.StartAction();
        Invoke("bsExit", duration);
    }
    public void bsExit() {
        if (action != null) action.Cancel();
        if(triggerId>-1) parent.Trigger(triggerId);
    }
    public override void Exit(string info = "")
    {
        base.Exit(info);
    }


}
