using CustomClasses;
using UnityEngine;
using UnityEngine.AI;

public class SlasherEnemy : StateEntity
{
	public Action attack;
    public float defSpeed;
    public MethodTrigger trigger;

    public override void Update(){
		base.Update();
	}
	
	
    public override void Start()
    {
        base.Start();
        //attack = GetComponent<AttackAction>();
        wander.StartActionRepeating();
    }
    public void TryAttack() {
        Debug.Log("triggered");
        attack.StartAction();
    }
    public void CheckTrigger() {
        if (trigger.inTrigger)
        {
            Debug.Log("Retriggered");
            attack.StartAction();
        }
    }
    public override void LocateTarget(GameObject target)
    {
        base.LocateTarget(target);
		//followTarget = true;
        agent.speed = defSpeed;
        GetComponent<StateMachine>().Trigger(0);
    }

}
