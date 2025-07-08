using CustomClasses;
using UnityEngine;
using UnityEngine.AI;

public class AccelerationChasingState : State
{
    NavMeshAgent agent;
    public float easingDuration, attackDelay, duration, maxSpeed;
    public float timePassed = 0;
    public int trigger = 1;
    public bool accelerating = false, deccelerating = false;
    public GameObject attackBox;
    float defSpeed;
    public Animator anim;
    public string animTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        base.Start();
    }
    public override void Enter(string info = "")
    {
        defSpeed = agent.speed;
        accelerating = true;
        timePassed = 0;
        Invoke("Attack", attackDelay);
        Invoke("Deccelerate", duration - easingDuration);
        Invoke("Finalize_", duration);
        if (anim != null) {
            anim.ResetTrigger(animTrigger);
            anim.SetTrigger(animTrigger);
        }
        base.Enter(info);
    }
    public void Finalize_() {
        agent.speed = defSpeed;
        

        parent.Trigger(trigger);
    }
    public void Attack() {
        attackBox.SetActive(true);
    }
    public void Deccelerate() {
        deccelerating = true;
        timePassed = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (accelerating) {
            timePassed += Time.deltaTime;
            agent.speed = (timePassed/easingDuration)*maxSpeed;
            if (agent.speed >= maxSpeed) 
            {
                agent.speed = maxSpeed;
                accelerating = false;
            }
        }

        if (deccelerating)
        {
            timePassed += Time.deltaTime;
            agent.speed = ((easingDuration-timePassed) / easingDuration) * maxSpeed;
            if (agent.speed <= defSpeed)
            {
                agent.speed = defSpeed;
                deccelerating = false;
            }
        }
    }
}
