using CustomClasses;
using System.Data;
using UnityEngine;

public class SlashSpell : Ability
{

    [SerializeField] private int cur = 0, n;
    [SerializeField] private float resetTime, resetTimer, diveCooldown = 0.5f, diveDelay = 0.2f, diveNudgeMultiplier = 1;
    [SerializeField] private Vector3 nudgeDown, nudgeUp, knockback, initialNudge;
    [SerializeField] private GameObject diveBox;
    [SerializeField] private bool didDive = false;


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
        if (player.grounded)
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
        else {
            //savedNudge = player.rb.linearVelocity;
            player.dashing = true;
            player.transform.rotation = player.abdir.transform.rotation;
            ApplyVelocity(transform, initialNudge);
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
        ApplyVelocity(transform, nudgeDown);
    }
    public override void OnSuccessfulHit(Damager dmg, DamageTrigger damageTrigger)
    {
        base.OnSuccessfulHit(dmg, damageTrigger);

        if (damageTrigger.cast == 0) ApplyVelocity(transform, Vector3.zero);//
        else
        {
            //player.Slowfall();
            ApplyVelocity(transform, nudgeUp);
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
