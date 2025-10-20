using UnityEngine;
using CustomClasses;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class DashSpell : Ability, IAbility
{
    public GameObject direction, storm;
    public float heavyInvTime = 1;
    public QuickSprint quickSprint;
    public Vector3 bounceUp, bounceBack;
    bool lockDirection = false;
    bool isDirectionDown = false;
    bool addSprint = false;
    [SerializeField] bool canBounce = true;
    StatusEffectManager effectManager;
    [SerializeField] float lookThreshold = 80;

    //[SerializeField] GameObject burstAttack;
    public override void Start()
    {
        base.Start();
        
        effectManager = GetComponent<StatusEffectManager>();
    }
    public override void Cast()
    {
        //Debug.Log("woosh");
        base.Cast();
        lockDirection = true;
        //effectManager.AddEffect(new QuickSprint());
        addSprint = true;
        Dash(CD[0], 0);
        attackBoxes[0].SetActive(true);
    }
    public override void Finish()
    {
        //quickSprint = new QuickSprint();
        if(addSprint)
        {
            addSprint = false;
            effectManager.AddEffect(quickSprint);
        }

        base.Finish();

    }
    public override void ResetVars()
    {
        base.ResetVars();

        canBounce = true;
        lockDirection = false;
        if(player.grounded) DirectionUp();
        else DirectionDown();
    }
    public void SetDirection() { 
        if(player.lookAngle < lookThreshold || player.grounded)
        {
            DirectionUp();
        }
        else
        {
            DirectionDown();
        }
    }
    public void DirectionUp() 
    {
        if (lockDirection) return;
        isDirectionDown = false;
        direction.transform.rotation = Quaternion.Euler(-45, direction.transform.rotation.eulerAngles.y, direction.transform.rotation.eulerAngles.z);
    }
    public void DirectionDown()
    {
        if (lockDirection) return;
        isDirectionDown = true;
        direction.transform.rotation = Quaternion.Euler(45, direction.transform.rotation.eulerAngles.y, direction.transform.rotation.eulerAngles.z);
    }
    public override void HeavyCast()
    {
        base.HeavyCast();
        if (player.dashing)
        {
            QueueCast(1);
            return;
        }

        SetDirection();
        lockDirection = true;
        //player.dashing = true;
        ready = false;
        CDleft = CD[1];
        CDset = CDleft;
        charge = minCharge;
        BeginCharging();
    }
    public override void ReleaseCharge()
    {
        
        if (charge < maxCharge)
        {
            charging = true;
            return;
        }
        SetDirection();

        base.ReleaseCharge();
        //effectManager.AddEffect(new QuickSprint());
        player.statusEffects.CleanseEffect("QuickSprint");
        lockDirection = true;
        player.queued = false;
        player.queuedCast = null;
        if (player.currentAbility != null) player.abilities[player.currentAbility.ID].Abort();

        attackBoxes[1].SetActive(true);
        Dash(CD[1], 1);
    }

    public override void OnSuccessfulHit(Damager dmg, DamageTrigger damageTrigger)
    {
        
        //return;

        if (!canBounce) return;
        canBounce = false;
        base.OnSuccessfulHit(dmg, damageTrigger);
        //player.hpManager.SetInvincibility(heavyInvTime);
        Debug.Log("successful heavy dash hit");

        //Abort();
        

        DashInfo s = dashInfo[currendDashIndex];
        if (isDirectionDown)
        {
            Instantiate(storm, player.transform.position, Quaternion.Euler(0, 0, 0));
            //ApplyVelocity(player.transform, bounceUp);
        }
        else
        {
            Instantiate(storm, player.transform.position, Quaternion.Euler(90, 0, 0));
            //ApplyVelocity(player.transform, bounceBack);
        }
        
    }
    void BurstAttack() { 
        
    }
}