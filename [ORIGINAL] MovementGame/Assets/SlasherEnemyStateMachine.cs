//using UnityEditorInternal;
using UnityEngine;
using CustomClasses;
using System.Collections.Generic;

public class SlasherEnemyStateMachine : CustomClasses.StateMachine
{
    public override void Start()
    {
        base.Start();
        entity.onDetectionEvent += DetectionTrigger;
    }
    public void DetectionTrigger(GameObject ignored) 
    {
        Trigger(0);
    }

    public void AttackTrigger()
    {
        Trigger(1);
    }

    public void AttackEndTrigger() {
        Trigger(2);
    }
}
