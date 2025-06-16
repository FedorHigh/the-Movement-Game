using UnityEditorInternal;
using UnityEngine;
using CustomClasses;
using System.Collections.Generic;

public class SlasherEnemyStateMachine : CustomClasses.StateMachine
{
    public void DetectionTrigger() 
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
