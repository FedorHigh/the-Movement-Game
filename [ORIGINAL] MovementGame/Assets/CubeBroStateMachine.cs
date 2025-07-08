using CustomClasses;
using UnityEngine;
using UnityEngine.AI;

public class DuoBossStateMachine : StateMachine
{
    //0 idle (0)
    //1 defensive attack
    //2 chasing (1)
    //3 chasing attack 1
    //4 chasing attack 2
    //5 follow-up attack
    //6 charging up (2)
    //7 stunned/retailation
    //8 mega attack

    //public float dangerLevel = 0, dangerDepletion = 1f, dangerThreshold = 100;
    public float aggressionLevel, aggressionGrowth = 1f, globalState = 0, aggressionThreshold = 100;
    //public float dangerFromDamage = 1, aggresiionFromDamage = 1;
    public float timePassed = 0, idleCD = 5, attackCD = 3;
    public bool attackReady = false, retreat = false, returnToDefense = false;
    public MethodTrigger trigga, bigTrigga;
    float curChance = 0;
    float defSpeed = 20, fastSpeed = 70;
    NavMeshAgent agent;
    
    

    public DuoBossStateMachine ally;

    public override void Switch(int target)
    {
        /*
        if (target == 2 & retreat) {
            retreat = false;
            Debug.LogError(label + " retreating!");
            target = 0;
        }
        if (target == 6 & retreat)
        {
            retreat = false;
            Debug.LogError(label + " retreating!");
            target = 0;
        }
        */

        if (target == 0)
        {
            globalState = 0;
            aggressionLevel = 0;
        }
        if (target == 2)
        {
            globalState = 1;
            aggressionLevel = 0;
        }
        if (target == 6) globalState = 2;
        if (target == 1 || target == 3 || target == 4) {
            attackReady = false;

        }
        base.Switch(target);
    }
    public virtual bool DoAltAttack() {
        if (Random.value >= curChance)
        {
            curChance += 0.2f;
            return false;
        }
        else {
            curChance = 0;
            return true;
        }

    }
    public virtual bool DoSmallAttack()
    {
        return Random.value >= 0.8f;
    }
    public override void Trigger(int id)
    {
        if ((id == 1 || id == 4) && !(attackReady && trigga.inTrigger)) return;
        if(id == 2) ally.Trigger(12);
        if (id == 13) {
            Debug.Log(label + " triggered 13");
            if (returnToDefense)
            {
                returnToDefense = false;
                Switch(0);
            }
            else Switch(2);
            return;
        }
        if (id == 10) {
            Debug.Log(label + " triggered 10");
            if (curState == 0)
            {
                if (ally.globalState != 1) Switch(2);
                else if (DoSmallAttack())
                {
                    returnToDefense = true;
                    aggressionLevel = aggressionThreshold * 0.5f;
                    Switch(5);
                }
                else Switch(6);
                return;
            }
            else if (curState == 2 && !bigTrigga.inTrigger) {
                Switch(5);
                return;
            }
            

        }
        if (id == 4) {
            Debug.Log(label + " triggered 4");
            if (curState != 2) return;
            if (DoAltAttack()) Switch(4);
            else Switch(3);
            return;
        }
        
        
        if (id == 9 && curState == 6)
        {
            ally.Trigger(11);
            /*Debug.Log("big ow");
            dangerLevel += entity.lastDamage * dangerFromDamage;
            if (globalState == 2) dangerLevel = 200;
            aggressionLevel += entity.lastDamage * aggresiionFromDamage;
            */
        }
        base.Trigger(id);
        if (id == 12 && curState == 0)
        {
            GetComponent<SmoothAnimate>().play();
        }
    }
    public void AnimOver() {
        agent.destination = transform.position;
    }
    public void ResolveAggression() {
        Debug.Log(label + " angry!");
        aggressionLevel = 0;
        Trigger(10);
        /*if (ally.globalState == 1) Trigger(6);
        else Trigger(2);
        */
    }
    public void Retreat() { 
        
    }
   
    public void Update() {
        if(curState==0 | !bigTrigga.inTrigger)aggressionLevel += aggressionGrowth * Time.deltaTime;
        if (aggressionLevel > aggressionThreshold) ResolveAggression();
        //if (ally.dangerLevel > dangerThreshold) AllyInDanger();
        //dangerLevel -= dangerDepletion * Time.deltaTime;
        //dangerLevel = Mathf.Max(0, dangerLevel);

        if (!attackReady) {
            timePassed += Time.deltaTime;
            if((timePassed>idleCD && curState == 0) || (timePassed>attackCD && curState == 2))
            {
                timePassed = 0;
                attackReady = true;
                if (curState == 0) Trigger(1);
                else Trigger(4);
            }
        }
    }
    public void InRange() {
        Debug.LogError(label + " in range");
        agent.speed = defSpeed;
        //dangerLevel += 50;
        /*if (globalState == 2)
        {
            Debug.LogError(label + " fear modified");
            dangerLevel = 200;
        }
        */
    }
    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Start();
        
    }
    public void OutOfRange() {
        Debug.LogError(label + " out of range");
        agent.speed = fastSpeed;
        /*dangerLevel -= 30;
        if (ally.globalState == 2)
        {
            Debug.LogError(ally.label + " fear modified");
            ally.dangerLevel = 200;
        }
        */
    }

}
