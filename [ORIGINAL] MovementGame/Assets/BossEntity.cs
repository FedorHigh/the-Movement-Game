using UnityEngine;
using UnityEngine.UI;

public class BossEntity : BaseEntity
{
    [SerializeField] public Slider healthbarboss;
    public LevelManager manager;
    public override void OnDeath()
    {
        manager.OnBossDeath();
        base.OnDeath();
    }
    private void UpdateHealthBar()
    {
        healthbarboss.value = hp / maxHp;
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        UpdateHealthBar();
    }

}
