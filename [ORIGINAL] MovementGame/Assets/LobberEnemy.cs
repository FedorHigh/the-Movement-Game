using UnityEngine;
using CustomClasses;

public class LobberEnemy : StateEntity
{
    public float cooldown;
    public RangedRepositionAction rep;
    public ThrowProjectileAction throwAttack;
    public int counter = 0;
    public override void LocateTarget(GameObject target)
    {
        base.LocateTarget(target);
        //state = 1;
        TargetObj = target;
        rep.target = target;
        throwAttack.TargetObj = target;
        GetComponent<StateMachine>().Trigger(0);
        //throwAttack.StartActionRepeating();
        //rep.StartActionRepeating();
        //followTarget = false;
    }
    //public void OutOfRange() {
    //    if (state == 0) return;
    //    followTarget = true;
    //    rep.Cancel();
    //    throwAttack.Cancel();
    //}
    //public void InRange() {
    //    if (state == 0) return;
    //    followTarget = false;
    //    rep.StartAction();
    //    throwAttack.StartActionRepeating();
    //}
    //public void RepReady() {
    //    if (state == 0) return;
    //    if (!followTarget) {
    //        rep.StartAction();
    //        throwAttack.Cancel();
    //    }
    //}
    //public void RepEnd() {
    //    if (state == 0) return;
    //    throwAttack.StartActionRepeating();
    //}
    public void DidAttack() {
        counter++;
        if (counter == 3) {
            counter = 0;
            GetComponent<StateMachine>().Trigger(2);
        }
    }
    public override void Start()
    {
        base.Start();
        //state = 0;
        
        rep = GetComponent<RangedRepositionAction>();
        throwAttack = GetComponent<ThrowProjectileAction>();
        //wander.StartActionRepeating();
    }
}
