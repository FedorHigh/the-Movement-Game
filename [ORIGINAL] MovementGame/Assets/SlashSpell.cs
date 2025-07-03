using CustomClasses;
using System.Data;
using UnityEngine;

public class SlashSpell : Ability
{
    
    public int cur = 0, n;
    public float resetTime, resetTimer, baseSpeed, slowSpeed = 25;


    public override void Start()
    {
        base.Start();
        n = attackBoxes.Length;
        baseSpeed = player.speed;
        cur = 0;
    }
    public override void Cast() {
        resetTimer = 0;
        player.transform.rotation = player.abdir.transform.rotation;
        attackBoxes[cur].SetActive(true);
        if (cur == 0)
        {
            //baseSpeed = player.speed;
            player.speed = slowSpeed;
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
            player.speed = baseSpeed;
            cur = 0;
            
        }
        else resetTimer += Time.deltaTime;
    }
    public override void Update() {

        if (player.dashing) player.speed = baseSpeed;
        base.Update();
        if(ready)WaitToReset();
    }
}
