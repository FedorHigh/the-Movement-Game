using CustomClasses;
using System.Data;
using UnityEngine;

public class SlashSpell : Ability
{

    [SerializeField] private int cur = 0, n;
    [SerializeField] private float resetTime, resetTimer, diveCooldown = 0.5f, diveDelay = 0.2f, diveNudgeMultiplier = 1;
    private bool canBounce = false;

    //dive attack
    [SerializeField] private Vector3 diveNudge, bounceNudge, knockback, diveNudgeInitial;
    [SerializeField] private GameObject diveBox;
    [SerializeField] private bool didDive = false;

    //dash attack
    [SerializeField] private Vector3 dashNudge, dashKnockback;
    [SerializeField] private GameObject dashBox;
    [SerializeField] private float dashDuration; 


    public override void Start()
    {
        base.Start();
        n = attackBoxes.Length;
        cur = 0;
        player.OnReachGround.AddListener(OnTouchGround);
    }
    public void OnTouchGround() {
        if (didDive)
        {
            SetCooldown(0.01f);
            didDive = false;
        }
    }
    public override void Cast() {
        base.Cast();
        if (player.statusEffects.CheckEffect("QuickSprint")) {

            canBounce = true;
            player.SetLocked(dashDuration);
            ApplyVelocity(transform, dashNudge);
            dashBox.SetActive(true);
            Debug.Log("DASH ATTACK!");
            player.statusEffects.CleanseEffect("QuickSprint");
        }
        else if (player.grounded)
        {
            resetTimer = 0;
            player.transform.rotation = player.abdir.transform.rotation;
            attackBoxes[cur].SetActive(true);
            if (cur == 0)
            {
                //baseSpeed = player.speed;
                player.forbidSprinting = true;
            }
            //
            SetCooldown(CD[cur]);
            //
            cur++;
            cur %= n;
        }
        else
        {
            //savedNudge = player.rb.linearVelocity;
            canBounce = true;
            player.dashing = true;
            player.transform.rotation = player.abdir.transform.rotation;
            ApplyVelocity(transform, diveNudgeInitial);
            Invoke("DiveAttack", diveDelay);
        }
    }
    void DiveAttack() {
        player.dashing = false;
        player.forbidSprinting = false;

        didDive = true;
        SetCooldown(diveCooldown);
        cur = 0;
        resetTimer = 0;

        diveBox.SetActive(true);
        ApplyVelocity(transform, diveNudge);
    }
    public override void OnSuccessfulHit(Damager dmg, DamageTrigger damageTrigger)
    {
  
        base.OnSuccessfulHit(dmg, damageTrigger);
        

        if (damageTrigger.cast == 0) ApplyVelocity(transform, Vector3.zero);//
        else if (damageTrigger.cast == 1 && canBounce)
        {
            //player.Slowfall();
            canBounce = false;
            ApplyVelocity(transform, bounceNudge);
        }
        else if (damageTrigger.cast == 2 && canBounce) {
            canBounce = false;
            ApplyVelocity(transform, dashKnockback);
        }
    }
    public void WaitToReset() {
        if (resetTimer >= resetTime) {
            player.forbidSprinting = false;
            cur = 0;
        }
        else resetTimer += Time.deltaTime;
    }
    public override void Update() {

        if (player.dashing) player.forbidSprinting = false;
        base.Update();
        if(ready)WaitToReset();
    }
}
