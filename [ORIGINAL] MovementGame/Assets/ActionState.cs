using CustomClasses;
using UnityEngine;

public class ActionState : State
{
    
    public Action action;
    public float duration, repetitionDelay = 0.2f;
    public int triggerId;
    public bool inherit = true, initDelay = false;
    public int repetitions = 1;

    public override void Start()
    {
        base.Start();
        if (inherit && action!=null) { 
            duration = action.duration;
            //triggerMethod = action.name + "EndTrigger";
        }
    }
    public void StartAction() {
        action.StartAction();
    }
    public override void Enter(string info = "")
    {
        base.Enter(info);
        if (action != null)
        {
            int I = 1;
            if (!initDelay) action.StartAction();
            else I--;
            float curDelay = repetitionDelay;
            for (int i = I; i < repetitions; i++) {
                Invoke("StartAction", curDelay);
                curDelay += repetitionDelay;
            }
            
        }
        
        Invoke("bsExit", duration);
    }
    public void bsExit() {
        
        if(triggerId>-1) parent.Trigger(triggerId);
        Debug.Log(parent.label + " triggered on action exit: " + triggerId);
        if (action != null) action.Cancel();
    }
    public override void Exit(string info = "")
    {
        base.Exit(info);
    }


}
