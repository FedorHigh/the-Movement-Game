using UnityEngine;
using CustomClasses;

public class FirstBossStateMachine : StateMachine
{

    public Transform player;
    public float attackCD, leftCD;
    public bool countDown;
    public MethodTrigger trigga;
    
    public override void Trigger(int id)
    {
        if (id == 0 && curState == 0) {
            if (!trigga.inTrigger)
            {
                Switch(2);
            }
            else {
                Switch(1);
            }
            return;
        }

        if (id == 1) {
            leftCD = attackCD;
            countDown = true;
        }

        base.Trigger(id);
    }
    public void Update()
    {
        if (countDown) {
            leftCD -= Time.deltaTime;
            if (leftCD <= 0) {
                countDown = false;
                Trigger(0);
            }
        }
    }
}
