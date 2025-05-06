using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class SlasherEnemy : StateEntity
{
	public AttackAction attack;
    //private float defSpeed;

    public override void Update(){
		base.Update();
	}
	
	
    public override void Start()
    {
        base.Start();
        attack = GetComponent<AttackAction>();
        wander.StartActionRepeating();
    }
    public void TryAttack() {
        attack.TryAttack();
    }
    public override void OnLocateTarget(GameObject target)
    {
        base.OnLocateTarget(target);
		followTarget = true;
        agent.speed = attack.defSpeed;
    }

}
