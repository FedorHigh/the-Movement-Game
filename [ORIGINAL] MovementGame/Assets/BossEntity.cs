using UnityEngine;

public class BossEntity : BaseEntity
{
    public LevelManager manager;
    public override void OnDeath()
    {
        manager.OnBossDeath();
        base.OnDeath();
    }
}
