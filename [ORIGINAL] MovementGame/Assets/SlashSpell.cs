using CustomClasses;
using System.Data;
using UnityEngine;

public class SlashSpell : Ability
{
    
    public int cur = 0, n;
    public float resetTime, resetTimer;


    public override void Start()
    {
        base.Start();
        n = attackBoxes.Length;
        cur = 0;
    }
    public override void Cast() {
        base.Cast();
        resetTimer = 0;
        player.transform.rotation = player.abdir.transform.rotation;
        attackBoxes[cur].SetActive(true);
        if (cur == 0)
        {
            //baseSpeed = player.speed;
            player.forbidSprinting = true;
        }
        //
        ready = false;
        CDleft = CD[cur];
        CDset = CD[cur];
        //
        cur++;
        cur %= n;
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
