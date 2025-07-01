using CustomClasses;
using UnityEngine;

public class RepeatedActionState : State
{
    public Action action;
    public float delay;
    public bool inherit = true;

    public override void Enter(string info = "")
    {
        base.Enter(info);
        if (action != null) Invoke("Begin", delay);
    }
    public void Begin()
    {
        action.StartActionRepeating();
    }
    public override void Exit(string info = "")
    {
        base.Exit(info);
        action.Cancel();
    }
}