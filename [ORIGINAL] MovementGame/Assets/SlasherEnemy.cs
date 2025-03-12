using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class SlasherEnemy : Entity
{
    public GameObject attack;
	public float CDleft, CDset, slowSpeed, defSpeed, threshold; 
	public bool attackReady;
	public float dist, dashStrength;
	public MethodTrigger trigger;
   	public override void Update(){
		base.Update();
		//dist = (TargetObj.transform.position - transform.position).magnitude;
		//if(dist <= threshold && attackReady){
			//doAttack();
		//}
	}
	public void TryAttack() {
		if (attackReady) doAttack();
	}
	public void resetMovement() {
        agent.speed = slowSpeed;
        //lookAtTarget = true;
		agent.enabled = true;
        rb.isKinematic = true;
    }
	public void resetAttack(){
		attackReady = true;
		if (trigger.inTrigger) doAttack();

        agent.speed = defSpeed;
    }
	public void doAttack(){
        agent.enabled = false;
		rb.isKinematic = false;
		DoLookAtTarget();
        rb.AddForce(transform.forward * dashStrength * rb.mass, ForceMode.Impulse);

		attackReady = false;

		
		Invoke("resetMovement", CDset * 0.5f);
		Invoke("resetAttack", CDset);
		attack.SetActive(true);
		//moveSpeed = 0;
		//lookAtTarget = false;
	}
    public override void Start()
    {
        base.Start();
		//locateAnyTarget(100000);
    }
    public override void OnLocateTarget(GameObject target)
    {
        base.OnLocateTarget(target);
		Debug.Log("SSSSSSSSSSEEEEEEYOUUUUUUUUUUUU");
		//GetComponent<WanderAround>().enabled = false;
		followTarget = true;
        agent.speed = defSpeed;
    }

}
