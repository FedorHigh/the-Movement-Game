using CustomClasses;
using UnityEngine;

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

    public float dangerLevel = 0, dangerDepletion = 1f, aggressionLevel, aggressionGrowth = 1f, globalState = 0, aggressionThreshold = 100, dangerThreshold = 100;
    public float dangerFromDamage = 1, aggresiionFromDamage = 1;
    public float timePassed = 0, idleCD = 5, attackCD = 3;
    public bool attackReady = false, retreat = false;
    public MethodTrigger trigga;
    public string label = "bro 1";

    

    public DuoBossStateMachine ally;

    public override void Switch(int target)
    {
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
        if (target == 0) globalState = 0;
        if (target == 2) globalState = 1;
        if (target == 6) globalState = 2;
        if (target == 1 || target == 3 || target == 4) {
            attackReady = false;

        }
        base.Switch(target);
    }
    public bool DoAltAttack() {
        return Random.value >= 0.5f;
    }
    public override void Trigger(int id)
    {
        if ((id == 1 || id == 4) && !(attackReady && trigga.inTrigger)) return;

        if (id == 4) {
            if (curState != 2) return;
            if (DoAltAttack()) Switch(4);
            else Switch(3);
            return;
        }
        
        if (id == 9)
        {
            Debug.Log("big ow");
            dangerLevel += entity.lastDamage * dangerFromDamage;
            if (globalState == 2) dangerLevel = 200;
            aggressionLevel += entity.lastDamage * aggresiionFromDamage;
        }
        base.Trigger(id);
    }
    public void ResolveAggression() {
        Debug.Log("angry!");
        aggressionLevel = 0;
        if (ally.globalState == 1) Trigger(6);
        else Trigger(2);
    }
    public void Retreat() { 
        
    }
    public void AllyInDanger() {
        
        if (curState == 0)
        {
            Debug.Log(label + " scared!");
            ally.dangerLevel = 0;
            ally.retreat = true;
            Trigger(2);
        }
        if (curState == 2) {
            Debug.LogError(label + " scared!");
            ally.dangerLevel = 0;
            ally.retreat = true;
            Trigger(5);
        }
    }
    public void Update() {
        if(curState == 0) aggressionLevel += aggressionGrowth * Time.deltaTime;
        if (aggressionLevel > aggressionThreshold) ResolveAggression();
        if (ally.dangerLevel > dangerThreshold) AllyInDanger();
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
        dangerLevel += 50;
        if (globalState == 2)
        {
            Debug.LogError(label + " fear modified");
            dangerLevel = 200;
        }
    }
    public void OutOfRange() {
        Debug.LogError(label + " out of range");
        dangerLevel -= 30;
        if (ally.globalState == 2)
        {
            Debug.LogError(ally.label + " fear modified");
            ally.dangerLevel = 200;
        }
    }

}
