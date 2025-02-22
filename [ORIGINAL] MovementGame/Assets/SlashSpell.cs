using Interfaces;
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
        resetTimer = 0;
        attackBoxes[cur].SetActive(true);
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
            cur = 0;
        }
        else resetTimer += Time.deltaTime;
    }
    public override void Update() {
        base.Update();
        if(ready)WaitToReset();
    }
}
