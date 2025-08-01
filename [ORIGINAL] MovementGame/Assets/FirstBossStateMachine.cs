using UnityEngine;
using CustomClasses;

public class FirstBossStateMachine : StateMachine
{
    //0 idle
    //1 dash
    //2 jump
    //3 shoot
    //4 idle 2

    public Transform player;
    public float attackCD, leftCD;
    float prevHP;
    public bool countDown;
    public MethodTrigger trigga;
    //Entity entity;

    public override void Start()
    {
        entity = GetComponent<Entity>();
        prevHP = entity.hp;
        base.Start();
    }
    public override void Switch(int target)
    {
        if (target == 0 && entity.hp <= (entity.maxHp * 0.5))
        {
            Debug.Log("2nd phase");
            target = 4;
        }
        base.Switch(target);
    }
    public override void Trigger(int id)
    {
        if (id == 0) {
            if (!trigga.inTrigger)
            {
                Switch(2);
            }
            else {
                if (Random.value < 0.3) Switch(3);
                else Switch(1);
            }
            return;
        }

        if (id == 1) {
            //leftCD = attackCD;
            //countDown = true;
        }

        base.Trigger(id);
    }
    public void Update()
    {
        /*
        if (countDown) {
            leftCD -= Time.deltaTime;
            if (leftCD <= 0) {
                countDown = false;
                Trigger(0);
            }
        }
        */
    }
}
