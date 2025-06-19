using CustomClasses;
using UnityEngine;

public class GrasshopperStateMachine : StateMachine
{
    // idle 
    // stomping (same state, action changes)
    // regular jump
    // frenzy jumps

    public Animator animator;
    public AttackAction[] attacks;
    public MethodTrigger[] triggerBoxes;
    public ActionState stompState;
    public float jumpCooldownMin, jumpCooldownMax;
    private float jumpCooldown;
    private bool jumpReady;
    private int curstomp = 0;
    

    public void changeAnimation(string animation, float crossfade = 0.2f) { 
        //
    }
    public void Trigger1() {
        Trigger(1);
    }
    public override void Trigger(int id)
    {
        if (id > 1 && id < 6) {

            curstomp = id - 2;
            stompState.action = attacks[curstomp];
        }
        base.Trigger(id);
    }
    public override void Start()
    {
        Invoke("PrepareJump", jumpCooldown);
        base.Start();
        
    }
    public void PrepareJump() {
        jumpCooldown = Random.Range(jumpCooldownMin, jumpCooldownMax);
        if (curState == 0) Trigger(0);
        else jumpReady = true;
    }
    public override void Switch(int target) {
        if (target == 1) {
            string[] s = { "L", "R", "F", "B" };
            foreach (string i in s)
            {
                animator.ResetTrigger(i);
            }
            animator.SetTrigger(s[curstomp]);
        }
        if (target == 2) {
            Invoke("PrepareJump", jumpCooldown);
        }


        base.Switch(target);

        if (target == 0)
        {
            if (jumpReady) { 
                jumpReady = false;
                Trigger(0);
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (triggerBoxes[i].inTrigger)
                {
                    Trigger(i+2);
                    break;
                }
            }
        }
    }
}
