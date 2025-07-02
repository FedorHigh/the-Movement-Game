using UnityEngine;
using CustomClasses;

public class FirstBossStateMachine : StateMachine
{

    public Transform player;
    public float attackCD, leftCD;
    public bool countDown;
    public MethodTrigger trigga;
    Entity entity;

    public override void Start()
    {
        entity = GetComponent<Entity>();
        base.Start();
    }
    public override void Switch(int target)
    {
        if (target == 0 && entity.hp <= (entity.maxHp * 0.5))
        {
            Debug.Log("2nd phase");
            target = 3;
            attackCD = 6;
            leftCD = 6;
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
                if (Random.value < 0.3) Switch(4);
                else Switch(1);
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
