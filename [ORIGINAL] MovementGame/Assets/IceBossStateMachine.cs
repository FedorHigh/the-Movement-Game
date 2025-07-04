using UnityEngine;
using CustomClasses;
using Cinemachine;

public class IceBossStateMachine : StateMachine
{
    // p1 following player
    // p1 slashing
    // p1 spinning
    // p2 repositioning
    // p2 attacking with laser

    public float slashCD = 2f, spinCD = 5f;
    public float time = 0, spinTime = 0;
    public float prevHP;
    Entity entity;
    public bool ticking = true, slashReady = false, spinReady = false;
    public MethodTrigger trigga;
    public override void Switch(int target)
    {
        if (target == 0 && entity.hp <= (entity.maxHp*0.2f))
        {
            target = 3;
        }
        base.Switch(target);
        if (target == 0)
        {
            if (spinReady)
            {
                Trigger(2);
                return;
            }
            time = 0;
            ticking = true;
            slashReady = false;
        }
    }
    public override void Trigger(int id)
    {
        if (id == 1) {
            spinTime = 0;
            spinReady = false;
        }
        if (id == 0 && !(slashReady && trigga.inTrigger)) return;
        
        base.Trigger(id);
    }
    public void Update() {
        //if (entity.hp < prevHP && curState == 4)
        //{
        //Debug.LogError("interrupted!");
        //prevHP = entity.hp;
        //Switch(3);
        //}
        if (curState == 0 && entity.hp <= (entity.maxHp * 0.2f))
        {
            Debug.Log("phase 2!");
            Switch(3);
        }
        prevHP = entity.hp;
        if (ticking) {
            time += Time.deltaTime;
            if (time >= slashCD) {
                ticking = false;
                slashReady = true;
                Trigger(0);
            }
        }
        if (!spinReady) {
            spinTime += Time.deltaTime;
            if (spinTime >= spinCD)
            {
                spinReady = true;
                Trigger(2);
            }
        }
    }
    public override void Start()
    {
        entity = GetComponent<Entity>();
        prevHP = entity.hp;
        base.Start();
    }
}
